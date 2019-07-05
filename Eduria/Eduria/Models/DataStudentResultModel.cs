using System;

namespace Eduria.Models
{
    public class DataStudentResultModel
    {
        public int ExamId { get; set; }
        public int ExamResultId { get; set; }
        public string ExamName { get; set; }
        public string ExamDescription { get; set; }
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
