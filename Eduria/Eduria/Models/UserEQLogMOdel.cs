using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class UserEQLogModel
    {
        public int UserEQLogId { get; set; }
        public int ExamHasQuestionId { get; set; }
        public QuestionModel QuestionModel { get; set; }
        public int ExamResultId { get; set; }
        public int UserId { get; set; }
        public int TimesWrong { get; set; }
        public DateTime AnsweredOn { get; set; }
        public int CorrectAnswered { get; set; }
    }
}
