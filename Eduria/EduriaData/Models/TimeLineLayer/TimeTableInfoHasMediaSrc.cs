using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.TimeLineLayer
{
    public class TimeTableInfoHasMediaSrc
    {
        [Key]
        public int TimeTableInfoHasMediaSrcId { get; set; }
        [Required]
        public int TimeTableInformationId { get; set; }
        [Required]
        public int MediaSourceId { get; set; }
    }
}
