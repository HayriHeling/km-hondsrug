using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class Exam
    {
        [Key]
        public int ExamId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
        [Required, MaxLength(45)]
        public string Name { get; set; }
        [Required, MaxLength(512)]
        public string Description { get; set; }
        [Required]
        public int IsActive { get; set; }
    }
}
