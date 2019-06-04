using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnalyticDefaultModel
    {
        public int AnalyticDefaultId { get; set; }
        public string AnalyticDefaultName { get; set; }
        public int AnalyticDefaultOption { get; set; }
        public int CategoryId { get; set; }
        public bool IsChecked { get; set; }
        public string Text { get; set; }

        public AnalyticDefaultModel()
        {
            IsChecked = false;
            Text = "";
        }
    }
}
