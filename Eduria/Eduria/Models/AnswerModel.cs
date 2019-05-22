using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public bool CorrectAnswer { get; set; }

        public AnswerModel()
        {

        }

        public AnswerModel(int id, int questionId, string text, bool correctAnswer)
        {
            this.Id = id;
            this.QuestionId = questionId;
            this.Text = text;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
