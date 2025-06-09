using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Models_and_Enums.Financial;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Reporting_and_Analytics.Services
{
	public class IncomeStatementMonthlyReportGenerator : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
        public IncomeStatementMonthlyReportGenerator(IServiceScopeFactory serviceScope)
        {
            _serviceScopeFactory = serviceScope;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceScopeFactory.CreateScope();

				var _particular_repo = scope.ServiceProvider.GetRequiredService<IParticularRepository>();
				var _database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

				await MonthlyIncomeStatementReport(_particular_repo,_database);
				await Task.Delay(TimeSpan.FromDays(12), stoppingToken);
			}			
		}

		private async Task MonthlyIncomeStatementReport(IParticularRepository particularRepository,DatabaseContext databaseContext)
		{
			var month_to_check = DateTime.Now.Month;
			var transaction = await databaseContext.Database.BeginTransactionAsync();

			try
			{
				var monthly_data_report = await databaseContext.Particulars.Where(i => i.isStored == false 
				                                                                 && i.transaction_date.Month == month_to_check)
					                                                       .Select(r => new
																		   {
																			   r.service,
																			   r.total_amount,
																			   r.isStored,
																			   r.transaction_id
																		   }).ToListAsync();
				foreach (var report in monthly_data_report)
				{
					var existing = await databaseContext.IncomeStatements.Where(i => i.service.Equals(report.service) && i.Month == month_to_check)
						                                                 .FirstOrDefaultAsync();

					if(existing != null)
					{
						existing.total_amount += report.total_amount;
						continue;
					}
					else
					{
						var statement_record = new IncomeStatement
						{
							service = report.service,
							total_amount = report.total_amount,
							isApproved = false,
							Month = month_to_check,
							year = DateTime.Now.Year,
						};
						databaseContext.IncomeStatements.Add(statement_record);
						await databaseContext.SaveChangesAsync();
					}                               
				}
				var list_of_id =  monthly_data_report.Select(i => i.transaction_id).ToList();

				foreach (var id in list_of_id)
				{
					var tracked_entity = new Particular { transaction_id = id , isStored = true};
					databaseContext.Attach(tracked_entity);
					databaseContext.Entry(tracked_entity).Property(i => i.isStored).IsModified = true;
				}
				await databaseContext.SaveChangesAsync();
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				Console.Write(ex.ToString());
				throw new ArgumentException();
			}
		}
	}
}
