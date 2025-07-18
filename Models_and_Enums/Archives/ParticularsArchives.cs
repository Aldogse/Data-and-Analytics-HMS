using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models_and_Enums.Archives
{
    public class ParticularsArchives
    {
        [Key]
        public Guid report_id { get; set; }
        public string service { get; set; }
        public decimal? total_amount { get; set; }
        public DateTime transaction_date { get; set; }
        public int Year { get; set; }
    }
}
