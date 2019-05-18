using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class UserTQLog
    {
        [Key]
        public int Id { get; set; }
        public Test Test { get; set; } 
        public Question Question { get; set; }
        public User User { get; set; }
        public int TimesWrong { get; set; }
    }
}
