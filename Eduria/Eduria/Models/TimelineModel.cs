using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TimelineModel
    {
        public string Name { get; set; }
        public List<TimeblockModel> TimeblockModels { get; set; }
    }
}
