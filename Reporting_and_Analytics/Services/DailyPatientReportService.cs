using Microsoft.EntityFrameworkCore;
using Models_and_Enums.patient_and_treatment_statistics;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Services
{
	public class DailyPatientReportService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
        public DailyPatientReportService(IServiceScopeFactory serviceScopeFactory)
        {
            _scopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _scopeFactory.CreateScope();
				var database =  scope.ServiceProvider.GetRequiredService<DatabaseContext>();

				await GenerateDailyPatientReport(database);
				await Task.Delay(TimeSpan.FromDays(12),stoppingToken);
			}
		}

		private async Task GenerateDailyPatientReport(DatabaseContext database)
		{
			var date_to_check = DateTime.Now.Day;
			var transaction = await database.Database.BeginTransactionAsync();
			try
			{
				var records_for_the_day = await database.PatientRecords.Where(i => i.admission_date.Day == date_to_check 
				                                                              && i.tracked == false).Select(i => new
																			  {
																				  i.patient_id,
																				  i.PHIC,
																				  i.type_of_service
																			  }).ToListAsync();
				var total_count = records_for_the_day.Count();
				int outpatient_count = records_for_the_day.Where(i => i.type_of_service ==
																	   Models_and_Enums.Enums.ServiceType.Outpatient).Count();
				int inpatient_count = records_for_the_day.Where(i => i.type_of_service ==
																Models_and_Enums.Enums.ServiceType.Inpatient).Count(); 

				int phic_members = records_for_the_day.Where(i => i.PHIC).Count();
				var patient_ids =  records_for_the_day.Select(i => i.patient_id).ToList();

				foreach (var id in patient_ids)
				{
					var tracked_data = new PatientRecords { patient_id = id, tracked = true };
					database.Attach(tracked_data);
					database.Entry(tracked_data).Property(i => i.tracked).IsModified = true;
				}

				var report = new DailyPatientReport
				{
					number_of_patients = total_count,
					total_inpatient = inpatient_count,
					total_outpatient = outpatient_count,
					phic_members = phic_members,
					report_date = DateTime.Now,
				};
				database.DailyPatientReports.Add(report);
				await database.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				throw new Exception($"Error while generating report:{ex.Message}");
			}
		}
	}
}
