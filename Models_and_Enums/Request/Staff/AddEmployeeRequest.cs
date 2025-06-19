using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Request.Staff
{
	public class AddEmployeeRequest
	{
		[Required]
		public string full_name { get; set; }
		[Required]
		public DateTime date_of_birth { get; set; }
		public decimal adherance_rate { get; set; }
		[Required]
		public DateTime shift_start { get; set; }
		[Required]
		public DateTime shift_end { get; set; }
		public Days start_off_day { get; set; }
		public Days end_off_day { get; set; }
	}
}
