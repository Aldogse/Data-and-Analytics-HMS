using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Services
{
	public class DailyIncomeReportService : BackgroundService
	{
		private readonly IServiceScopeFactory _scopeFactory;
		public DailyIncomeReportService(IServiceScopeFactory serviceScopeFactory)
		{
			_scopeFactory = serviceScopeFactory;
		}
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _scopeFactory.CreateScope();
				var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				await DailyIncomeStatementReportGenerator(database);
				await Task.Delay(TimeSpan.FromDays(12),stoppingToken);
			}
		}

		private async Task DailyIncomeStatementReportGenerator(DatabaseContext database)
		{
			var day_to_check = DateTime.Now.Day - 1;
			decimal? total = 0;
			try
			{
				var records = await database.Particulars.Where(i => i.transaction_date.Day == day_to_check 
				                                              && i.transaction_date.Month == DateTime.Now.Month)
					                                          .Select(r => new
															  {
																  r.total_amount,
															  }).ToListAsync(); 
				foreach(var item in records)
				{
					total += item.total_amount;
				}

				var day_report = new DailyIncomeReport
				{
					day = day_to_check,
					month = DateTime.Now.Month,
					total_income = total,
				};

				database.DailyIncomeRecords.Add(day_report);
				await database.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				throw new Exception($"Error: {ex.Message}");
			}
		}



	}
}
