using System.Data;
using Microsoft.AspNetCore.Mvc;
using Models_and_Enums.Financial;
using Models_and_Enums.Responses.IncomeStatement;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
	[ApiController]
	[Route("api/income-statement/")]
	public class IncomeStatementController : ControllerBase
	{
        private readonly DatabaseContext _databaseContext;
        private readonly IIncomeStatementRepository _incomeStatementRepository;
        public IncomeStatementController(DatabaseContext databaseContext,IIncomeStatementRepository incomeStatementRepository)
        {
            _databaseContext = databaseContext;
            _incomeStatementRepository = incomeStatementRepository;
        }

        [HttpGet("get-all-records")]
        public async Task <IActionResult> get_all_records()
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);

            try
            {
                var records = await _incomeStatementRepository.IncomeStatementsRecords();

                if(records.Count <= 0 && records == null)
                {
                    return Ok(records);
                }

                var response = new MonthlyIncomeStatementResponse
                {
                    month = records[0].Month,
                    statements = records.Select(i => new IncomeStateRecordsResponse
                    {
                        total_amount = i.total_amount,
                        report_id = i.report_id,
                        service = i.service,                       
                    }).ToList()                
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");
                throw new Exception();
            }
        }
        [HttpDelete("delete-statement/{report_id}")]
        public async Task<IActionResult> DeleteReport(int report_id)
        {

            var report_to_delete = await _incomeStatementRepository.GetReportById(report_id);
            _databaseContext.IncomeStatements.Remove(report_to_delete);

            await _databaseContext.SaveChangesAsync();
            return Ok(report_to_delete);
        }
        [HttpGet("get-records-by-month/{month}")]
        public async Task<IActionResult> GetRecordsByMonth(int month)
        {
            try
            {
                var records_of_month = await _incomeStatementRepository.IncomeStatementByMonth(month);

                if(records_of_month.Count <= 0 || records_of_month == null)
                {
                    return NotFound();
                }

                var response = new MonthlyIncomeStatementResponse
                {
                    month = month,
                    statements = records_of_month.Select(i => new IncomeStateRecordsResponse
                    {
                        total_amount = i.total_amount,
                        report_id = i.report_id,
                        service = i.service
                    }).ToList()
                };

                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
    }
}
