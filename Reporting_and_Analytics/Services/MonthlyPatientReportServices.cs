using Microsoft.EntityFrameworkCore;
using Models_and_Enums.patient_and_treatment_statistics;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Migrations;

namespace Reporting_and_Analytics.Services
{
	public class MonthlyPatientReportServices : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
        public MonthlyPatientReportServices(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			using var scope = _scopeFactory.CreateScope();
			var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

			await MonthlyPatientReport(database);
			await Task.Delay(TimeSpan.FromMinutes(1),stoppingToken);
		}

		private async Task MonthlyPatientReport(DatabaseContext database)
		{
			var transaction = await database.Database.BeginTransactionAsync();
			try
			{
				var month_transaction = await database.PatientRecords.Where(i => i.admission_date.Month == DateTime.Now.Month)
																	 .ToListAsync();

				var services_count = month_transaction.GroupBy(i => i.type_of_service)
													  .ToDictionary(i => i.Key,i => i.Count());

				var total = month_transaction.Count();

				var inpatient_count = services_count.GetValueOrDefault(Models_and_Enums.Enums.ServiceType.Inpatient);
				var outpatient_total = services_count.GetValueOrDefault(Models_and_Enums.Enums.ServiceType.Outpatient);

				var total_patient = month_transaction.Count();
				var phic_members = month_transaction.Where(i => i.PHIC == true).Count();

				var ages = month_transaction.Select(i => i.Age).ToList();
				int total_age = 0;

				foreach(var age in ages)
				{
					total_age += age;
				}

				var average_age = total_age / total;

				var monthly_patient_record = new MonthlyPatientReport
				{
					total_inpatient = inpatient_count,
					total_outpatient = outpatient_total,
					phic_members = phic_members,
					Average_age = average_age,
					month = DateTime.Now.Month,
					number_of_patients = total_patient,
					report_generated = DateTime.Now,
				};
				database.MonthlyPatientReports.Add(monthly_patient_record);
				await database.SaveChangesAsync();

				await transaction.CommitAsync();
			}
			catch (DbUpdateException ex)
			{
				await transaction.RollbackAsync();
				throw new DbUpdateException(ex.Message);
			}
		}
	}
}
