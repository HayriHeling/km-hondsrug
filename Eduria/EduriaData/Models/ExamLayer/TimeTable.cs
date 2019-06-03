using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class TimeTable
    {
        [Key]
        public int TimeTableId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Source { get; set; }
    }
}
