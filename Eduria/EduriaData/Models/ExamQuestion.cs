using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class ExamQuestion
    {
        [Key]
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int QuestionId { get; set; }
    }
}