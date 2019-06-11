using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class PeriodModel
    {
        public int PeriodNum { get; set; }
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public int SchoolYearStart { get; set; }
        public int SchoolYearEnd { get; set; }
    }
}
