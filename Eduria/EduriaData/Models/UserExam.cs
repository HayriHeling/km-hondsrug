using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class UserExam
    {
        [Key]
        public int Id { get; set; }
        public Exam Exam { get; set; }
        public User User { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
