using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        [Required]
        public int TimeTableId { get; set; }
        [Required, MaxLength(200)]
        public string Text { get; set; }
        [MaxLength(500)]
        public string MediaLink { get; set; }
        [Required]
        public int MediaType { get; set; }
        [Required]
        public int QuestionType { get; set; }
    }
}
