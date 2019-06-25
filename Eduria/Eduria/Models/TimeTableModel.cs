using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TimeTableModel
    {
        public int TimeTableId { get; set; }
        public string Text { get; set; }
        public string TimeTableDesignId { get; set; }
        public string Description { get; set; }
        public MediaSourceModel MediaSourceModel { get; set; }
    }
}
