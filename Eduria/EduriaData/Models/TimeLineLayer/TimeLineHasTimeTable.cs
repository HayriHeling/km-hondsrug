using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeLineHasTimeTable
    {
        [Key]
        public int TimeLineHasTimeTableId { get; set; }
        [Required]
        public int TimeLineId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
    }
}
