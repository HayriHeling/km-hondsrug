using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class TimeblockModel
    {
        public TimeTableModel TimeTableModel { get; set; }
        public List<TimeBlockInformationModel> TimeBlockInformationModels { get; set; }
    }
}
