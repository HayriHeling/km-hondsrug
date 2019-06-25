namespace Eduria.Models
{
    public class QuestionAnswerModel
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public int AnswerId { get; set; }
        public string AnswerText { get; set; }
        public int GivenAnswerId { get; set; }
        public string GivenAnswerText { get; set; }
    }
}
