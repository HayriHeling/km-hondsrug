using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class ExamResultModel
    {
        public int ExamResultId { get; set; }
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
    }
}
