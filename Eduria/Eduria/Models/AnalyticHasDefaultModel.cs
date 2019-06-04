using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnalyticHasDefaultModel
    {
        public int AnalyticDefaultId { get; set; }
        public int AnalyticDataId { get; set; }
        public string AnalyticDefaultName { get; set; }
        public int CategoryId { get; set; }
        public int? Score { get; set; }
        public string Input { get; set; }
    }
}
