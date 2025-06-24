using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models_and_Enums.Enums;
using Models_and_Enums.Request.Staff;
using Models_and_Enums.Responses.Staff;
using Models_and_Enums.Staff;
using Reporting_and_Analytics.Data;
using Reporting_and_Analytics.Interface;

namespace Reporting_and_Analytics.Controllers
{
	public class EmployeeController : ControllerBase
	{
        private readonly DatabaseContext _databaseContext;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(DatabaseContext databaseContext,IEmployeeRepository employeeRepository)
        {
            _databaseContext = databaseContext;
            _employeeRepository = employeeRepository;
        }
        private static string GenerateEmployeeId(string full_name,DateTime date_of_birth)
        {
            var initials = string.Concat(full_name.Split(' ',StringSplitOptions.RemoveEmptyEntries)
                                                  .Select(i => i[0]))
                                                  .ToUpper();
            var dob = date_of_birth.ToString("MMddyy");
            var random = new Random();
            var random_number = random.Next(4,99);

            return $"EID-{initials}{dob}{random_number}";
        }


        [HttpGet("get-employee-details")]
        public async Task<IActionResult> GetEmployees()
        {
            try
            {
                var employee_details = await _databaseContext.Employees.Select(i => new
                { 
                                                                        i.employee_id,
                                                                        i.full_name,
                                                                        i.date_of_birth,
                                                                        i.shift_start_formatted,
                                                                        i.shift_end_formatted,
                                                                        i.start_off_day,
                                                                        i.end_off_day
                }).ToListAsync();

                if(employee_details == null)
                {
                    return NotFound();
                }

                var response = employee_details.Select(i => new EmployeeDetailsResponse
                {
                    full_name = i.full_name,
                    date_of_birth = i.date_of_birth.ToShortDateString(),
                    shift_start = i.shift_start_formatted,
                    shift_end = i.shift_end_formatted,
                    start_off_day = i.start_off_day.ToString(),
                    end_off_day = i.end_off_day.ToString()
                }).ToList();

                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }

		[HttpGet("employee-details/{employee_id}")]
        public async Task<IActionResult> EmployeeDetails(string employee_id)
        {
            try
            {
                var employee_details = await _employeeRepository.get_employee_by_Id(employee_id);

                if (employee_details == null)
                {
                    return NotFound();
                }

                var response = new EmployeeDetailsResponse
                {
                    full_name = employee_details.full_name,
                    date_of_birth = employee_details.date_of_birth.ToShortDateString(),
                    shift_start = employee_details.shift_start_formatted,
                    shift_end = employee_details.shift_end_formatted,
                    start_off_day = Enum.GetName(typeof(Days), employee_details.start_off_day),
                    end_off_day = Enum.GetName(typeof(Days), employee_details.end_off_day),
                };

                return Ok(response);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpPost("create-employee")]
        public async Task<IActionResult> CreateEmployee([FromBody]AddEmployeeRequest new_employee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var employee = new Employee
                {
                    full_name = new_employee.full_name.ToUpper(),
                    date_of_birth = new_employee.date_of_birth,
                    adherance_rate = 100,
                    shift_start = new TimeSpan(new_employee.shift_start_hour,new_employee.shift_start_minute,0),
                    shift_end =  new TimeSpan(new_employee.shift_end_hour,new_employee.shift_end_minute,0),
                    end_off_day = new_employee.end_off_day,
                    start_off_day = new_employee.start_off_day,
                    employee_id = GenerateEmployeeId(new_employee.full_name,new_employee.date_of_birth)
                };

               _databaseContext.Employees.Add(employee);
               await _databaseContext.SaveChangesAsync();
                return Ok();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
            catch(NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
        [HttpDelete("delete-employee/{employee_id}")]
        public async Task<IActionResult> DeleteEmployee(string employee_id)
        {
            try
            {
                var employee_to_delete = await _employeeRepository.get_employee_by_Id(employee_id);

                if(employee_to_delete != null)
                {
                    _databaseContext.Employees.Remove(employee_to_delete);
                    await _databaseContext.SaveChangesAsync();
                    return Ok();
                }
                return NotFound();
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.Message);
            }
        }

        [HttpGet("first-name-search/{first_name}")]
        public async Task<IActionResult> FirstNameSearch(string first_name)
        {
            try
            {
                var data =   _databaseContext.Employees.AsEnumerable().Where(i => i.first_name == first_name).ToList();

                if (data.Count > 0 || data != null)
                {
                   var response = data.Select(i => new EmployeeDetailsResponse
                   {
                       employee_id = i.employee_id,
                       full_name = i.full_name,
                       date_of_birth = i.date_of_birth.ToShortDateString(),
                       shift_start = i.shift_start_formatted,
                       shift_end = i.shift_end_formatted,
                       start_off_day = i.start_off_day.ToString(),
                       end_off_day = i.end_off_day.ToString()
                   }).ToList();
                   return Ok(response);
                }

                return NotFound();

               
			}
            catch (NullReferenceException ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("last-name-search/{last_name}")]
        public async Task<IActionResult> LastNameSearch(string last_name)
        {
            try
            {
                var employee_last_names =  _databaseContext.Employees.AsEnumerable()
                                                                     .Where(i => i.last_name == last_name)
                                                                     .ToList();
                if(employee_last_names.Count > 0 || employee_last_names != null)
                {
                    var response = employee_last_names.Select(i => new EmployeeDetailsResponse
                    {
                        employee_id = i.employee_id,
                        full_name = i.full_name,
                        date_of_birth = i.full_name.ToString(),
                        shift_start = i.shift_start_formatted,
                        shift_end = i.shift_end_formatted,
                        start_off_day = i.start_off_day.ToString(),
                        end_off_day = i.end_off_day.ToString()
                    }).ToList();

                    return Ok(response);
                }
                return NotFound();
            }
            catch (NullReferenceException ex)
            {
                throw new NullReferenceException(ex.Message);
            }
        }
    }
}
