using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Financial
{
	public  class Particular
	{
        [Key]
        public int transaction_id { get; set; }
        public string service { get; set; }
        public decimal? total_amount { get; set; }
        public DateTime transaction_date { get; set; }
        public bool isStored { get; set; }
    }
}
