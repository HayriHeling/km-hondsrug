using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class AnswerService : AService<Answer>
    {
        public AnswerService(EduriaContext context)
        {
            this.Context = context;
        }

        public override IEnumerable<Answer> GetAll()
        {
            return Context.Answers;
        }

        public override Answer GetById(int id)
        {
            return Context.Answers.Find(id);
        }

        /// <summary>
        /// This method is not used at this moment, but may very well be used later on.
        /// The method uses a list of questions to search for the answers that belong to the questions.
        /// </summary>
        /// <param name="questions">List of Question-models</param>
        /// <returns>List of Answer-models</returns>
        public IEnumerable<Answer> GetAnswersByQuestionsList(IEnumerable<Question> questions)
        {
            IEnumerable<Answer> answers = GetAll();
            List<Answer> tempAnswers = new List<Answer>();
            foreach (Question question in questions)
            {
                foreach(Answer answer in answers.Where(x => x.QuestionId == question.QuestionId))
                {
                    tempAnswers.Add(answer);
                }
            }
            return tempAnswers;
        }
    }
}
