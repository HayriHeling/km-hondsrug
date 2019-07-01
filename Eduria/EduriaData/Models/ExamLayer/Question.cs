using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
        [Required]
        public int MediaSourceId { get; set; }
        [Required, StringLength(int.MaxValue)]
        public string Text { get; set; }
        [Required]
        public int QuestionType { get; set; }
    }
}
