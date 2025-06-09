using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Responses.DailyReport
{
	public class DailyReportResponse
	{
        public int total_patient { get; set; }
        public int total_inpatient { get; set; }
        public int total_outpatient { get; set; }
        public int total_phic { get; set; }
        public string report_date { get; set; }
    }
}
