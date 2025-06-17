using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Enums;
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
                var month_to_check = await _databaseContext.IncomeStatements.Where(i => i.Month == month && i.year == DateTime.Now.Year)
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
                    month = Enum.GetName(typeof(Month), month),
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

        [HttpDelete("delete-income-record/{id}")]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            var income_to_delete = await _databaseContext.HospitalIncomeRecords.Where(i => i.report_id == id).FirstOrDefaultAsync();

            if(income_to_delete == null)
            {
                return NotFound();
            }

            _databaseContext.HospitalIncomeRecords.Remove(income_to_delete);
            await _databaseContext.SaveChangesAsync();
            return Ok(income_to_delete);
        }


        [HttpGet("first-quarter-income")]
        public async Task<IActionResult> first_quarter_range_income()
        {
            try
            {
                var months = new HashSet<int> { 1,2,3,4,5,6};
                var records = await _databaseContext.HospitalIncomeRecords.Where(i => months.Contains(i.month)).OrderBy(i => i.month).ToListAsync();

                var response = records.Select(i => new MonthlyHospitalIncomeRecordResponse
                {
                    month = Enum.GetName(typeof(Month),i.month),
                    total_income = i.total_income,
                }).ToList();
                                                                          
                if(records.Count <= 0 || records == null)
                {
                    return NotFound();
                }
                
                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

    }
}
