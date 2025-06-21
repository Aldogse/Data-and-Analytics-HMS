using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Staff
{
	public class Employee
	{
        [Key]
		public string employee_id {  get; set; }
        [Required]
        public string full_name { get; set; }
		[Required]
		public DateTime date_of_birth { get; set; }
        public decimal adherance_rate { get; set; }
		[Required]
		public TimeSpan shift_start { get; set; }
		[JsonIgnore]
		public string shift_start_formatted => DateTime.Today.Add(shift_start).ToString("hh:mm tt");
		[Required]
		public TimeSpan shift_end { get; set; }
		[JsonIgnore]
		public string shift_end_formatted => DateTime.Today.Add(shift_end).ToString("hh:mm tt");
        public Days start_off_day { get; set; }
        public Days end_off_day { get; set; }
    }
}
