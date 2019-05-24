using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models
{
    public class ExamResult
    {
        [Key]
        public int ExamResultId { get; set; }
        [Required]
        public int ExamId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
