using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Request.Staff;
using Models_and_Enums.Staff;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthorizationController : ControllerBase
	{
        private readonly IAppUserCredentials _appUserCredentials;
		private readonly DatabaseContext _context;
		private readonly IEmployeeRepository _employeeRepository;

		public AuthorizationController(IAppUserCredentials appUserCredentials,DatabaseContext context,IEmployeeRepository employeeRepository)
        {
            _appUserCredentials = appUserCredentials;
			_context = context;
			_employeeRepository = employeeRepository;
		}

        [HttpPost("Register")]
        public async Task<IActionResult> AppUserRegistration([FromBody]RegisterUserCredentialsRequest registerUserCredentials)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

				var userEmail = await _employeeRepository.GetEmployeeByEmail(registerUserCredentials.Email);

				if (userEmail == null)
				{
					return NotFound();
				}

				var newUser = await _appUserCredentials.Register(registerUserCredentials);
                return Ok(newUser);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500,ex.Message);
            }
            catch (NullReferenceException ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpPost("LogIn")]
        public async Task <IActionResult> LogIn([FromBody]AppUserCredentials userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userEmail = await _employeeRepository.GetEmployeeByEmail(userCredentials.Email);
	   var LogInAttempt = await _appUserCredentials.Login(userCredentials);
            if(LogInAttempt)
            {
                var token = _appUserCredentials.GenerateSecurityStringToken(userEmail);
                return Ok(token);
            }

            return BadRequest();
        }
    }
}
