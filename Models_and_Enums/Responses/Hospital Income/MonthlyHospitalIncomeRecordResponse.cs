using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Responses.Hospital_Income
{
	public class MonthlyHospitalIncomeRecordResponse
	{
        public int month { get; set; }
        public decimal? total_income { get; set; }
    }
}
