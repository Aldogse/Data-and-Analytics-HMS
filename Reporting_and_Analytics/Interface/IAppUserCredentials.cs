using Models_and_Enums.Request.Staff;
using Models_and_Enums.Staff;

namespace Reporting_and_Analytics.Interface
{
	public interface IAppUserCredentials
	{
		Task<bool> Login(AppUserCredentials userCredentials);
		Task<bool> Register(RegisterUserCredentialsRequest registerUserCredentials);
		string GenerateSecurityStringToken(Employee EmployeeCredentials);
	}
}
