using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Financial;
using Models_and_Enums.Responses.Hospital_Income;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
    [ApiController]
    [Route("api/HospitalIncomeRecords/")]
    public class HospitalIncomeRecordsController : ControllerBase
    {
        private readonly IIncomeStatementRepository _incomeStatementRepository;
        private readonly DatabaseContext _databaseContext;
        private readonly IParticularRepository _particularRepository;
        public HospitalIncomeRecordsController(IIncomeStatementRepository incomeStatementRepository,
                                               DatabaseContext databaseContext,
                                               IParticularRepository particularRepository)
        {
            _databaseContext = databaseContext;
            _incomeStatementRepository = incomeStatementRepository;
            _particularRepository = particularRepository;
        }

        [HttpGet("get-monthly-income/{month}")]
        public async Task<IActionResult> GetMonthIncome(int month)
        {
            try
            {
                var month_to_check = await _databaseContext.IncomeStatements.Where(i => i.Month == month)
                                                                            .Select(r => new
                                                                            {
                                                                                r.total_amount,
                                                                            }).ToListAsync();

                decimal? total = 0;
                foreach (var record in month_to_check)
                {
                    total += record.total_amount;
                }

                var month_check = await _databaseContext.HospitalIncomeRecords.Where(i => i.month == month && i.year == DateTime.Now.Year)
                                                                              .FirstOrDefaultAsync();

                var income_record = new HospitalIncomeRecords
                {
                    total_income = total,
                    month = month,
                    year = DateTime.Now.Year
                };
                _databaseContext.Add(income_record);
                await _databaseContext.SaveChangesAsync();

                var response = new MonthlyHospitalIncomeRecordResponse
                {
                    month = month,
                    total_income = total,
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("get-yearly-income/{year}")]
        public async Task<IActionResult> GetYearlyIncome(int year)
        {
            try
            {
                decimal? total = 0;
                var year_income = await _databaseContext.IncomeStatements.Where(i => i.year == year)
                                                                         .Select(i => new
                                                                         {
                                                                             i.total_amount
                                                                         }).ToListAsync();

                if(year_income.Count <=0 || year_income == null)
                {
                    return NotFound();
                }

                foreach(var item in year_income)
                {
                    total += item.total_amount;  
                }

                var response = new YearlyHospitalIncomeRecordsResponse
                {
                    year = year,
                    total = total
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
