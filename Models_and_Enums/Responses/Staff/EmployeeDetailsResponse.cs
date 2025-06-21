using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;



namespace Models_and_Enums.Responses.Staff
{
	public class EmployeeDetailsResponse
	{
		public string full_name { get; set; }
		public string date_of_birth { get; set; }
		public string shift_start { get; set; }
		public string shift_end { get; set; }
		public string start_off_day { get; set; }
		public string end_off_day { get; set; }
	}
}
