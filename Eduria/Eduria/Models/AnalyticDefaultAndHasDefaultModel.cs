using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnalyticDefaultAndHasDefaultModel
    {
        public AnalyticDataModel AnalyticData { get; set; }
        public List<AnalyticHasDefaultModel> AnalyticHasDefaultModels { get; set; }
        public List<AnalyticDefaultModel> AnalyticDefaultModels { get; set; }
    }
}
