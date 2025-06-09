using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Request.Particulars
{
	public  class ParticularAddRequest
	{
		[Required]
		public string service { get; set; }
		public decimal? total_amount { get; set; }
		[Required]
		public DateTime transaction_date { get; set; }
	}
}
