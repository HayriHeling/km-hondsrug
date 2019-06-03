using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class AnswerT
    {
        [Key]
        public int AnswerTId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string Source { get; set; }
    }
}
