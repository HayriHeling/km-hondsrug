﻿using System.Collections.Generic;

namespace Eduria.Models
{
    public class DataExamResultModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalTimesDone { get; set; }
        public List<int> TotalTimesDonePerMonth { get; set; }
        public int HighestScore { get; set; }
        public int LowestScore { get; set; }
        public double AverageScore { get; set; }
    }
}
