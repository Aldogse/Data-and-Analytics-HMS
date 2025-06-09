using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Services
{
	public class ParticularTableDataCleanUpService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
        public ParticularTableDataCleanUpService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using var scope = _serviceScopeFactory.CreateScope();
				var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
				//var particular_repo = scope.ServiceProvider.GetRequiredService<IParticularRepository>();
				await DataAdjustmentMethod(database);
				await Task.Delay(TimeSpan.FromDays(12),stoppingToken);
			}
		}

		private async Task DataAdjustmentMethod(DatabaseContext database)
		{
			var year_checker = DateTime.Now.Year - 3;
			try
			{
				var records_to_be_delete = await database.Particulars.Where(i => i.transaction_date.Year == year_checker).ToListAsync();			
				database.RemoveRange(records_to_be_delete);
				await database.SaveChangesAsync();
			}
			catch (DbUpdateException ex)
			{
				throw new DbUpdateException($"Error: {ex.Message}");
			}
		}

		
	}
}
