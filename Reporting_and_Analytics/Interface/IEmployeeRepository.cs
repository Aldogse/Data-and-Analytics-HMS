using Models_and_Enums.Staff;

namespace Reporting_and_Analytics.Interface
{
	public interface IEmployeeRepository
	{
		Task<Employee> get_employee_by_Id(string employee_id);
	}
}
