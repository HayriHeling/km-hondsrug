using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EduriaData.Models
{
    public class UserTest
    {
        [Key]
        public int Id { get; set; }
        public Test Test { get; set; }
        public User User { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
