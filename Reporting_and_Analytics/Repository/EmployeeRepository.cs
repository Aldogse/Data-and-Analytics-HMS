using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Staff;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Repository
{	
	public class EmployeeRepository : IEmployeeRepository
	{
		private readonly DatabaseContext _databaseContext;
		public EmployeeRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

		public async Task<List<Employee>> GetEmployees()
		{
			return await _databaseContext.Employees.OrderByDescending(i => i.full_name).ToListAsync();
		}

		public async Task<Employee> get_employee_by_Id(string employee_id)
		{
			return await _databaseContext.Employees.Where(i => i.employee_id == employee_id).FirstOrDefaultAsync();
		}

	}
}
