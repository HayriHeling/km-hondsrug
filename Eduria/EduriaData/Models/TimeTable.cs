using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class TimeTable
    {
        [Key]
        public int TimeTableId { get; set; }
        [Required]
        public string TimeTableDesignId { get; set; }
        [Required]
        public string Text { get; set; }
        public string Description { get; set; }
        [Required]
        public int MediaSourceId { get; set; }
    }
}
