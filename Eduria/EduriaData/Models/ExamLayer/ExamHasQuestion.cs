using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class ExamQuestion
    {
        [Key]
        public int ExamHasQuestionId { get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public int QuestionId { get; set; }
    }
}
