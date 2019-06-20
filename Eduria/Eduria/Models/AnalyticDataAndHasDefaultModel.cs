using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnalyticDataAndHasDefaultModel
    {
        public AnalyticDataModel AnalyticData { get; set; }
        public List<AnalyticHasDefaultModel> AnalyticHasDefaultModels { get; set; }
    }
}