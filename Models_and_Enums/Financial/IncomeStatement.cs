using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Models_and_Enums.Enums;

namespace Models_and_Enums.Financial
{
    public class IncomeStatement
    {
        [Key]
        public int report_id { get; set; }
        public string service { get; set; }
        public bool isApproved { get; set; }
        public decimal? total_amount { get; set; }
        public int Month { get; set; }
        public int? year { get; set; }
    }
}
