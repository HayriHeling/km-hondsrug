using System.Collections.Generic;

namespace Eduria.Models
{
    public class TimeblockModel
    {
        public TimeTableModel TimeTableModel { get; set; }
        public List<TimeBlockInformationModel> TimeBlockInformationModels { get; set; }
    }
}
