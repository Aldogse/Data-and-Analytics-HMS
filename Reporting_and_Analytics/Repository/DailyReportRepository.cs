using Microsoft.EntityFrameworkCore;
using Models_and_Enums.patient_and_treatment_statistics;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{
	public class DailyReportRepository : IDailyPatientReportRepository
	{
		private readonly DatabaseContext _databaseContext;
        public DailyReportRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

		public async Task<DailyPatientReport> GetReportByDay(int month,int day)
		{
			var record_to_check = await _databaseContext.DailyPatientReports.Where(i => i.report_date.Month == month &&
			                                                        i.report_date.Day == day).FirstOrDefaultAsync();
			return record_to_check;
		}

		public async Task<DailyPatientReport> GetReportById(int report_id)
		{
			var record_to_check = await _databaseContext.DailyPatientReports.Where(i => i.report_id
																			   == report_id).FirstOrDefaultAsync();
			return record_to_check;
		}
	}
}
