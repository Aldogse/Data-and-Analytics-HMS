using Models_and_Enums.patient_and_treatment_statistics;

namespace Reporting_and_Analytics.Interface
{
	public interface IDailyPatientReportRepository
	{
		Task<DailyPatientReport> GetReportById(int report_id);
		Task<DailyPatientReport>GetReportByDay(int month,int day);
	}
}
