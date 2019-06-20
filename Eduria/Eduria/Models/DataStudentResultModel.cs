using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class DataStudentResultModel
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int ExamId { get; set; }
        public int ExamResultId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
    }
}
