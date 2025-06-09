using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Responses.IncomeStatement
{
	public  class IncomeStateRecordsResponse
	{
		public int report_id { get; set; }
		public string service { get; set; }
		public decimal? total_amount { get; set; }
    }
}
