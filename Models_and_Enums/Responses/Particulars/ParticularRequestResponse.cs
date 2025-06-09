using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Responses.Particulars
{
	public class ParticularRequestResponse
	{
		public int transaction_id { get; set; }
		public string service { get; set; }	
		public decimal? total_amount { get; set; }
		public string transaction_date { get; set; }
	}
}
