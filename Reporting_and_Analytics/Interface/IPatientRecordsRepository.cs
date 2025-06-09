using Models_and_Enums.patient_and_treatment_statistics;

namespace Reporting_and_Analytics.Interface
{
	public interface IPatientRecordsRepository
	{
		Task<List<PatientRecords>> GetAllPatientsRecords();
		Task<List<PatientRecords>> GetPatientRecordsByDay(int day);
		Task<PatientRecords> GetPatientRecordsById(Guid patient_id);
		Task<PatientRecords> GetPatientRecordsByFullName(string full_name);
	}
}
