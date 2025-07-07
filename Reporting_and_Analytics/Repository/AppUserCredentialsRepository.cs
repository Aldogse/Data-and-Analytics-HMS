using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models_and_Enums.Request.Staff;
using Models_and_Enums.Staff;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{
	public class AppUserCredentialsRepository : IAppUserCredentials
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly IConfiguration _config;
        public AppUserCredentialsRepository(UserManager <IdentityUser> userManager,SignInManager <IdentityUser> signInManager,IConfiguration config)
        {
            _userManager = userManager;
			_signInManager = signInManager;
			_config = config;
        }

		public string GenerateSecurityStringToken(Employee EmployeeCredentials)
		{
			
			var claims = new List<Claim>()
			{
				new Claim(ClaimTypes.Name,EmployeeCredentials.full_name),
				new Claim(ClaimTypes.Email,EmployeeCredentials.Email),
			};
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value));
			var signingCredentials = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256Signature);

			var securityToken = new JwtSecurityToken(
				claims:claims,
				audience: _config.GetSection("JWT:Audience").Value,
				issuer:_config.GetSection("JWT:Issuer").Value,
				signingCredentials: signingCredentials
				);

			var SecurityTokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);
			return SecurityTokenString;
		}

		public async Task<bool> Login(AppUserCredentials userCredentials)
		{
			var userEmail = (await _userManager.FindByEmailAsync(userCredentials.Email));
			
			if(userEmail == null)
			{
				return false;
			}

			if (await _userManager.CheckPasswordAsync(userEmail, userCredentials.Password))
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		public async Task<bool> Register(RegisterUserCredentialsRequest registerUserCredentials)
		{
			var newUser = new IdentityUser
			{
				Email = registerUserCredentials.Email,
				UserName = registerUserCredentials.Email
			};

			var result = await _userManager.CreateAsync(newUser,registerUserCredentials.Password);

			if(result.Succeeded)
			{
				return true;  
			}

			return false;
		}
	}
}
