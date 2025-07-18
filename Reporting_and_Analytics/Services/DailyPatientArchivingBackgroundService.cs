
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models_and_Enums.Archives;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Services
{
    public class DailyPatientArchivingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScope;

        public DailyPatientArchivingBackgroundService(IServiceScopeFactory serviceScope)
        {
            _serviceScope = serviceScope;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope =  _serviceScope.CreateScope();

                var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await DailyPatientArchivingMethod(database);

                await Task.Delay(TimeSpan.FromHours(12),stoppingToken);
            }
        }

        //Function
        private async Task DailyPatientArchivingMethod(DatabaseContext database)
        {
            string format = "dd/MM/yyyy";
            string dateToCheck = DateTime.Now.AddYears(-1).ToString();
            var transaction = await database.Database.BeginTransactionAsync();
            try
            {
                DateTime.TryParseExact(dateToCheck, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out var extractedDate);

                var dailyPatient = await database.DailyPatientReports.Where(i => EF.Functions
                                                                    .DateDiffDay(i.report_date, extractedDate) == 0)
                                                                    .ToListAsync();

                //convert item into Daily patient archives object
                var archives = dailyPatient.Select(i => new DailyPatientReportArchives
                {
                    number_of_patients = i.number_of_patients,
                    report_date = i.report_date,
                    phic_members = i.phic_members,
                    total_inpatient = i.total_inpatient,
                    total_outpatient = i.total_outpatient,
                }).ToList();

                await database.DailyPatientReportArchives.AddRangeAsync(archives);
                database.DailyPatientReports.RemoveRange(dailyPatient);

                await database.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                throw new DbUpdateException(ex.Message);
            }
            catch (NullReferenceException ex)
            {
                await transaction.RollbackAsync();
                throw new NullReferenceException(ex.Message);
            }
        }
    }
  
}
