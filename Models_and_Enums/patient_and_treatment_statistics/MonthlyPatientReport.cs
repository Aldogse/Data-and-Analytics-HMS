using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.patient_and_treatment_statistics
{
	public class MonthlyPatientReport
	{
		[Key]
		public int report_id { get; set; }
		[Required]
		public int number_of_patients { get; set; }
		[Required]
		public int total_inpatient { get; set;}
		[Required]
		public int total_outpatient { get; set; }
		[Required]
		public DateTime report_generated { get; set; }
		[Required]
		public int month { get; set; }
		public int phic_members { get; set; }
	}
}
