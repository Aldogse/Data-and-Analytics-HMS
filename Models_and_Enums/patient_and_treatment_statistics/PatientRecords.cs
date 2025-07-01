using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.patient_and_treatment_statistics
{
	public class PatientRecords
	{
		[Key]
		public Guid patient_id { get;set ; }
		[Required]
        public string Full_name { get; set; }
        [JsonIgnore]
        public string first_name => Full_name.Split(' ').First().ToLower();
        [JsonIgnore]
        public string last_name => Full_name.Split(' ').Last().ToLower();
        [Required]
        public int Age { get; set; }
        [Required]
        public Gender Sex { get; set; }
        public DateTime admission_date { get; set; }
		public bool PHIC { get; set; }
        public ServiceType  type_of_service { get; set; }
        public bool tracked { get; set; }
    }
}
