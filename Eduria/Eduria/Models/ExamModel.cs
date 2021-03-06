﻿using System.Collections.Generic;

namespace Eduria.Models
{
    public class ExamModel
    {
        public int ExamId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ExamResultId { get; set; }
        public int Score { get; set; }
        public int MaxScore { get; set; }
        public TimeTableModel TimeTable { get; set; }
        public List<QuestionModel> QuestionModels;
        public List<AnswerModel> AnswerModels;
    }
}
