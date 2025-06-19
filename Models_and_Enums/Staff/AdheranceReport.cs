using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Models_and_Enums.Staff
{
	public class AdheranceReport
	{
        [Key]
        public int report_id { get; set; }
        [ForeignKey("Employee")]
        public string employee_id { get; set; }
        [JsonIgnore]
        public Employee Employee { get; set; }
        public DateTime clock_in { get; set; }
        public DateTime clock_out { get; set; }
        public DateTime report_date { get; set; }
    }
}
