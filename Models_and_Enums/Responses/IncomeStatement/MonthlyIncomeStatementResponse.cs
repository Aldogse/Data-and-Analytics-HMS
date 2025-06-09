using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models_and_Enums.Financial;

namespace Models_and_Enums.Responses.IncomeStatement
{
	public class MonthlyIncomeStatementResponse
	{
        public int month { get; set; }
        public List<IncomeStateRecordsResponse> statements { get; set; }
    }
}

