using System.Collections.Generic;

namespace Eduria.Models
{
    public class AnalyticDefaultAndHasDefaultModel
    {
        public AnalyticDataModel AnalyticData { get; set; }
        public List<AnalyticHasDefaultModel> AnalyticHasDefaultModels { get; set; }
        public List<AnalyticDefaultModel> AnalyticDefaultModels { get; set; }
    }
}
