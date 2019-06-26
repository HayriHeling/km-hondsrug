using System.Collections.Generic;

namespace Eduria.Models
{
    public class TimelineModel
    {
        public string Name { get; set; }
        public List<TimeblockModel> TimeblockModels { get; set; }
    }
}
