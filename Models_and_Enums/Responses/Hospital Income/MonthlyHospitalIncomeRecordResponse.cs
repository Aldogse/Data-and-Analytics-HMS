using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Responses.Hospital_Income
{
	public class MonthlyHospitalIncomeRecordResponse
	{
        public string month { get; set; }
        public decimal? total_income { get; set; }
    }
}
