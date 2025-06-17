using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Models_and_Enums.patient_and_treatment_statistics;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Services
{
	public class MonthlyHospitalIncomeReportService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
        public MonthlyHospitalIncomeReportService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceScopeFactory.CreateScope();
				var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				await HospitalIncomeService(database);
				await Task.Delay(TimeSpan.FromDays(12),stoppingToken);
			}
		}

		//function
		public async Task HospitalIncomeService(DatabaseContext databaseContext)
		{
			var month_to_check = DateTime.Now.Month - 1;
			try
			{
				var total_amount = await databaseContext.IncomeStatements.Where(i => i.Month == month_to_check)
																		 .SumAsync(i => i.total_amount);

				var report = new HospitalIncomeRecords
				{
					total_income = total_amount,
					month = month_to_check,
					year = DateTime.Now.Year,					
				};

				databaseContext.HospitalIncomeRecords.Add(report);
				await databaseContext.SaveChangesAsync();
				
			}
			catch (DbUpdateException ex)
			{
				throw new DbUpdateException(ex.Message);
			}
		}
	}
}
