using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class UserEQLog
    {
        [Key]
        public int Id { get; set; }
        public int ExamId { get; set; } 
        public int QuestionId { get; set; }
        public int UserId { get; set; }
        public int TimesWrong { get; set; }
    }
}
