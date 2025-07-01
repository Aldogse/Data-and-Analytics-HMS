using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Responses.PatientReport
{
	public class PatientDetailsResponse
	{
		public Guid patient_id { get; set; } 
		public string Full_name { get; set; }
		[Required]
		public int Age { get; set; }
		[Required]
		public string Sex { get; set; }
		public string admission_date { get; set; }
		public bool PHIC { get; set; }
		public string type_of_service { get; set; }
	}
}
