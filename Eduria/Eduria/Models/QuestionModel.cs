﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class QuestionModel
    { 
        public int QuestionId { get; set; }
        public int QuestionType { get; set; }
        public string Text { get; set; }
        public MediaType MediaType { get; set; }
        public string MediaLink { get; set; }
        public int AnswerId { get; set; }
        public TimeTableModel TimeTable { get; set; }
    }
}
