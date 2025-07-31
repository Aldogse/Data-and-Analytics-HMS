using Microsoft.AspNetCore.Mvc;
using Models_and_Enums.Responses.DailyReport;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
	[ApiController]
	[Route("api/DailyReport/")]
	public class DailyPatientReportController : ControllerBase
	{
		private readonly DatabaseContext _databaseContext;
		private readonly IPatientRecordsRepository _patientRecordsRepository;
		private readonly IDailyPatientReportRepository _dailyPatientReportRepository;
		public DailyPatientReportController(DatabaseContext databaseContext,
									 IPatientRecordsRepository patientRecordsRepository,
									 IDailyPatientReportRepository dailyPatientReportRepository)
		{
			_dailyPatientReportRepository = dailyPatientReportRepository;
			_databaseContext = databaseContext;
			_patientRecordsRepository = patientRecordsRepository;
		}

		[HttpDelete("delete-report/{report_id}")]
		public async Task<IActionResult> DeleteReportById(int report_id)
		{
			try
			{
				var report_to_delete = await _dailyPatientReportRepository.GetReportById(report_id);
				if (report_to_delete == null)
				{
					return NotFound();
				}

				_databaseContext.Remove(report_to_delete);
				await _databaseContext.SaveChangesAsync();
				return Ok();
			}
			catch (DbUpdateException ex)
			{
				return StatusCode(500, ex.Message);
			}
		}

		[HttpGet("get-day-report/{month}/{day}")]
		public async Task<IActionResult> GetReportByday(int month, int day)
		{
			try
			{
				 
				var report_request = await _dailyPatientReportRepository.GetReportByDay(month,day);

				//check if the report exist
				if(report_request == null)
				{
					return NotFound();
				}

				var response = new DailyReportResponse
				{
					total_patient = report_request.number_of_patients,
					total_inpatient = report_request.total_inpatient,
					total_outpatient = report_request.total_outpatient,
					total_phic = report_request.phic_members,
					report_date = report_request.report_date.ToShortDateString(),
				};

				return Ok(response);
			}
			catch (NullReferenceException ex)
			{
				return StatusCode(500, ex.Message);
			}
		}
	}
}
