
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Archives;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Services
{
    public class PatientRecordArchivingBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScope;

        public PatientRecordArchivingBackgroundService(IServiceScopeFactory serviceScope)
        {
            _serviceScope = serviceScope;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
           using var scope = _serviceScope.CreateScope();
            var database = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            await PatientRecordsArchivingTask(database);

            await Task.Delay(TimeSpan.FromHours(12));
        }

        //function
        private async Task PatientRecordsArchivingTask(DatabaseContext database)
        {
            var transaction = await database.Database.BeginTransactionAsync();
            var format = "dd/MM/yyyy";
            var dateToCheck = DateTime.Today.AddYears(-3).ToString();
            try
            {
                DateTime.TryParseExact(dateToCheck,format,CultureInfo.InvariantCulture,DateTimeStyles.None,out var extractedDate);
                var patientRecords = await database.PatientRecords.Where(i => EF.Functions
                                                                  .DateDiffDay(i.admission_date,extractedDate) == 0)
                                                                  .ToListAsync();

                var archives = patientRecords.Select(i => new PatientRecordArchives
                {
                    admission_date = i.admission_date,
                    Age = i.Age,
                    Full_name = i.Full_name,
                    patient_id = i.patient_id,
                    PHIC = i.PHIC,
                    Sex = i.Sex,
                    tracked = i.tracked,
                    type_of_service = i.type_of_service,
                }).ToList();

                await database.PatientRecordArchives.AddRangeAsync(archives);
                database.PatientRecords.RemoveRange(patientRecords);

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
