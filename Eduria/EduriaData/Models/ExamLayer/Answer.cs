using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        [Required]
        public int QuestionId { get; set; }
        [Required, MaxLength(200)]
        public string Text { get; set; }
        [Required]
        public int Correct { get; set; }
    }
}
