using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reporting_and_Analytics.Data;

namespace Reporting_and_Analytics.Controllers
{
	[ApiController]
	[Route("api/daily-income/")]
	public class DailyIncomeController : ControllerBase
	{
		private readonly DatabaseContext _databaseContext;
		public DailyIncomeController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		[HttpDelete("delete-report/{id}")]
		public async Task<IActionResult> DeleteReport(int id)
		{
			try
			{
				var report_to_delete = await _databaseContext.DailyIncomeRecords.Where(i => i.report_id == id).FirstOrDefaultAsync();

				if(report_to_delete != null)
				{
					 _databaseContext.Remove(report_to_delete);
					await _databaseContext.SaveChangesAsync();
					return Ok(report_to_delete);
				}

				  return NotFound();
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

	}
}
