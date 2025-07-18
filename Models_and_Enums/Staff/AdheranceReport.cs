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
        public TimeSpan clock_in { get; set; }
        [JsonIgnore]
        public string clock_in_string => DateTime.Today.Add(clock_in).ToString("hh:mm tt");
        public TimeSpan clock_out { get; set; }
        [JsonIgnore]
        public string clock_out_string => DateTime.Today.Add(clock_out).ToString("hh:mm tt");
        public DateTime report_date { get; set; }
    }
}
