using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnswerModel
    {
        public int Id;
        public string Text;
        public bool CorrectAnswer;

        public AnswerModel()
        {

        }

        public AnswerModel(int id, string text, bool correctAnswer)
        {
            this.Id = id;
            this.Text = text;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
