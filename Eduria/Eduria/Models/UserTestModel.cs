using System;

namespace Eduria.Models
{
    /// <summary>
    /// UserTest model with the Exam and User data.
    /// </summary>
    public class UserTestModel
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ExamName { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
