﻿namespace Eduria.Models
{
    public class QuestionModel
    { 
        public int QuestionId { get; set; }
        public MediaSourceModel MediaSourceModel { get; set; }
        public int QuestionType { get; set; }
        public string Text { get; set; }
        public TimeTableModel TimeTableModel { get; set; }
        public int CorrectAnswered { get; set; }
    }
}
