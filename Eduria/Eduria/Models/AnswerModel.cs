namespace Eduria.Models
{
    public class AnswerModel
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }
    }
}
