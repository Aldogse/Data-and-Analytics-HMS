using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Runtime.TagHelpers;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Models_and_Enums.Financial;
using Models_and_Enums.Request.Particulars;
using Models_and_Enums.Responses.Particulars;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
	[ApiController]
	[Route("api/particular/")]
	public class ParticularController : ControllerBase
	{
        private readonly IParticularRepository _particularRepository;
        private readonly DatabaseContext _databaseContext;
        public ParticularController(IParticularRepository particularRepository, DatabaseContext databaseContext)
        {
            _particularRepository = particularRepository;
            _databaseContext = databaseContext;
        }

        //CRUD FUNCTIONS
        [HttpGet("get-all-transactions")]
        public async Task<IActionResult> GetAllTransactions()
        {
           if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var transactions = await _particularRepository.GetAllServices();

                if(transactions.Count() <= 0 || transactions == null)
                {
                    return Ok(transactions);
                }

                var response = transactions.Select(item => new ParticularRequestResponse
                {
                    service = item.service,
                    total_amount = item.total_amount,
                    transaction_date = item.transaction_date.ToShortDateString(),
                    transaction_id = item.transaction_id,
                }).ToList();

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("get-transaction-year/{year}")]
        public async Task <IActionResult> GetTransactionByYear(int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var yearly_transaction = await _particularRepository.GetServicesByYear(year);

                if(yearly_transaction == null || yearly_transaction.Count() >= 0)
                {
                    return Ok(yearly_transaction);
                }

                var response = yearly_transaction.Select(item => new ParticularRequestResponse
                {
                    transaction_id = item.transaction_id,
                    total_amount = item.total_amount,
                    service = item.service,
                    transaction_date = item.transaction_date.ToShortTimeString(),
                }).ToList();

                return Ok(response);
            }
            catch(Exception)
            {
                throw new Exception();
            }
        }

        [HttpGet("get-transaction-month")]
        public async Task <IActionResult> GetTransactionByMonth(int month)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var monthly_transaction = await _particularRepository.GetServicesByMonth(month);

                if(monthly_transaction.Count() >= 0 || monthly_transaction == null)
                {
                    return Ok(monthly_transaction);
                }

                var response = monthly_transaction.Select(item => new ParticularRequestResponse
                {
                    transaction_id = item.transaction_id,
                    total_amount = item.total_amount,
                    service = item.service,
                    transaction_date = item.transaction_date.ToShortTimeString(),
                  
                }).ToList();

                return Ok(response);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [HttpPost("new-transaction")]
        public async Task<IActionResult> new_transaction([FromBody]ParticularAddRequest particularAddRequest)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var new_service = new Particular
                {
                    service = particularAddRequest.service,
                    total_amount = particularAddRequest.total_amount,
                    transaction_date = particularAddRequest.transaction_date
                };

                _databaseContext.Particulars.Add(new_service);
                await _databaseContext.SaveChangesAsync();

                var response = new ParticularRequestResponse
                {
                    service = new_service.service,
                    total_amount=new_service.total_amount,
                    transaction_date = new_service.transaction_date.ToShortDateString(),
                    transaction_id = new_service.transaction_id
                };

                return Ok(response);                
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

        [HttpPut("update-service-information/{transaction_id}")]
        public async Task<IActionResult> updateInformation(int transaction_id, [FromBody]UpdateParticularRequest updateParticularRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var service_to_update = await _particularRepository.GetServiceById(transaction_id);

                //check if the service exist
                if(service_to_update == null)
                {
                    return NotFound();
                }

                service_to_update.service = updateParticularRequest.service;
                service_to_update.total_amount = updateParticularRequest.total_amount; 

                _databaseContext.Particulars.Update(service_to_update);
                await _databaseContext.SaveChangesAsync();

                var response = new ParticularRequestResponse
                {
                    total_amount = service_to_update.total_amount,
                    service = service_to_update.service,
                    transaction_date = service_to_update.transaction_date.ToShortDateString(),
                    transaction_id = transaction_id
                };

                return Ok(response);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
    }
}
