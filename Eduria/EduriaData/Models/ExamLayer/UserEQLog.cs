using System;
using System.ComponentModel.DataAnnotations;

namespace EduriaData.Models.ExamLayer
{
    public class UserEQLog
    {
        [Key]
        public int UserEQLogId { get; set; }
        [Required]
        public int ExamHasQuestionId { get; set; }
        [Required]
        public int ExamResultId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int TimesWrong { get; set; }
        public DateTime AnsweredOn { get; set; }
        public int CorrectAnswered { get; set; }
    }
}
