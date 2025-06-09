using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Financial
{
	public  class HospitalIncomeRecords
	{
		[Key]
        public int report_id { get; set; }
        public decimal? total_income { get; set; }
        public int month { get; set; }
        public int year { get; set; }
    }
}
