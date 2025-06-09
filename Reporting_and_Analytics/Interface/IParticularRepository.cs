using Models_and_Enums.Financial;

namespace Reporting_and_Analytics.Interface
{
	public interface IParticularRepository
	{
		Task<List<Particular>> GetAllServices();
		Task<List<Particular>> GetServicesByMonth(int month);
		Task <List<Particular>> GetServicesByYear(int year);
		Task<Particular> GetServiceById(int service_id);
		
	}
}
