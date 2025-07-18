using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Archives
{
    public class DailyIncomeReportArchive
    {
        [Key]
        public int report_id { get; set; }
        [Required]
        public decimal? total_income { get; set; }
        [Required]
        public int day { get; set; }
        [Required]
        public int month { get; set; }
    }
}
