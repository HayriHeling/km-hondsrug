using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnswerModel
    {
        public int Id;
        public int QuestionId;
        public string Text;
        public bool CorrectAnswer;

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
