using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Request.PatientReport
{
	public class AddNewPatientRequest
	{
		public string Full_name { get; set; }

		public int Age { get; set; }
		public Gender Sex { get; set; }
		public DateTime admission_date { get; set; }
		public bool PHIC { get; set; }
		public ServiceType type_of_service { get; set; }
	}
}
