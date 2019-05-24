using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class QuestionHasAnswerT
    {
        [Key]
        public int QuestionHasAnswerTId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required]
        public int AnserTId { get; set; }
    }
}
