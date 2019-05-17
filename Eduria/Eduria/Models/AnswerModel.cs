using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Models
{
    public class AnswerModel
    {
        public string Text;
        public bool CorrectAnswer;

        public AnswerModel(string text, bool correctAnswer)
        {
            this.Text = text;
            this.CorrectAnswer = correctAnswer;
        }
    }
}
