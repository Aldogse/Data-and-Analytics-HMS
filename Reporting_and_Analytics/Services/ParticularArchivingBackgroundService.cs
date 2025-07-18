
using System.Globalization;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Models_and_Enums.Financial;
using Reporting_and_Analytics.Data;
using Models_and_Enums.Archives;

namespace Reporting_and_Analytics.Services
{
    public class ParticularArchivingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _service;

        public ParticularArchivingBackgroundService(IServiceScopeFactory service)
        {
            _service = service;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var service =  _service.CreateAsyncScope();
                var dataBase = service.ServiceProvider.GetRequiredService<DatabaseContext>();

                await ParticularArchiving(dataBase);
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ParticularArchiving(DatabaseContext database)
        {
            string format = "dd/MM/yyyy";
            string dateToCheck = DateTime.Now.AddYears(-1).ToString("dd/MM/yyyy");
            try
            {
                var transaction = await database.Database.BeginTransactionAsync();
                DateTime.TryParseExact(dateToCheck, format, CultureInfo.InvariantCulture, DateTimeStyles.None,
                                                                                          out var DateToCheck);

                var particulars = await database.Particulars.Where(i => EF.Functions.DateDiffDay(i.transaction_date, DateToCheck) == 0)
                    .ToListAsync();
                //convert particulars item to particular archive object 
                var archives = particulars.Select(i => new ParticularsArchives
                {
                    service = i.service,
                    total_amount = i.total_amount,
                    transaction_date = i.transaction_date,
                    Year = i.transaction_date.Year
                }).ToList();

                await database.ParticularsArchives.AddRangeAsync(archives);
                database.Particulars.RemoveRange(particulars);

                await database.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }
    }
}
