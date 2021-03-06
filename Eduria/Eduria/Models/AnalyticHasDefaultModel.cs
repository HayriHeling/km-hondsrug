﻿namespace Eduria.Models
{
   public class AnalyticHasDefaultModel
    {
        public int DataHasDefaultId { get; set; }
        public int AnalyticDefaultId { get; set; }
        public int AnalyticDataId { get; set; }
        public string AnalyticDefaultName { get; set; }
        public int CategoryId { get; set; }
        public int? Score { get; set; }
        public string Input { get; set; }
        public int Option { get; set; }
    }
}
