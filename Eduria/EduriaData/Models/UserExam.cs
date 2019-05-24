using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class UserExam
    {
        [Key]
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
