using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class UserTQLog
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Id")]
        public int TestId { get; set; }
        [ForeignKey("Id")]
        public int QuestionId { get; set; }
        [ForeignKey("Id")]
        public int UserId { get; set; }
        public int TimesWrong { get; set; }
    }
}
