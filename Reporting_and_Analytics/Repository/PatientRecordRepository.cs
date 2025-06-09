using Microsoft.EntityFrameworkCore;
using Models_and_Enums.patient_and_treatment_statistics;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{
	public class PatientRecordRepository : IPatientRecordsRepository
	{
		private readonly DatabaseContext _databaseContext;
        public PatientRecordRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

		public async Task<List<PatientRecords>> GetAllPatientsRecords()
		{
			return await _databaseContext.PatientRecords.ToListAsync();
		}

		public async Task<List<PatientRecords>> GetPatientRecordsByDay(int day)
		{
			return await _databaseContext.PatientRecords.AsNoTracking().Where(i => i.admission_date.Day == day
														   && i.admission_date.Month == DateTime.Now.Month && i.tracked == false).ToListAsync();
		}

		public async Task<PatientRecords> GetPatientRecordsByFullName(string full_name)
		{
			var patient = await _databaseContext.PatientRecords.Where(i => i.Full_name.ToLower().Equals(full_name.ToLower())).FirstOrDefaultAsync();
			return patient;
		}

		public async Task<PatientRecords> GetPatientRecordsById(Guid patient_id)
		{
			return await _databaseContext.PatientRecords.Where(i => i.patient_id == patient_id).FirstOrDefaultAsync();
		}
	}
}
