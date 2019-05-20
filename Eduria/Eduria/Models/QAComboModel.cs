using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models.Interfaces;

namespace Eduria.Models
{
    public class QAComboModel
    {
        public IQuestion Question;
        public List<AnswerModel> AnswerModels;

        public QAComboModel(int id, string questionText, string questionMedia = "", int questionType=0)
        {
            this.Question = new TextQuestionModel(id, questionText);
        }

        public QAComboModel(int id, string questionText, List<AnswerModel> answerModels)
        {
            Question = new TextQuestionModel(id, questionText);
            AnswerModels = answerModels;
        }

        public QAComboModel(IQuestion question, List<AnswerModel> answerModels)
        {
            this.Question = question;
            this.AnswerModels = answerModels;
        }

        /// <summary>
        /// Probably not used atm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="correctAnswer"></param>
        public void AddAnswer(int id, string text, bool correctAnswer)
        {
            AnswerModels.Add(new AnswerModel(id, text, correctAnswer));
        }
    }
}
