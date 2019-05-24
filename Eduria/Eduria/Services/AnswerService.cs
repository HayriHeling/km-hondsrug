using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

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

        public IEnumerable<Answer> GetAnswersByQuestionsList(IEnumerable<Question> questions)
        {
            IEnumerable<Answer> answers = GetAll();
            List<Answer> tempAnswers = new List<Answer>();
            foreach (Question question in questions)
            {
                foreach(Answer answer in answers.Where(x => x.QuestionId == question.Id))
                {
                    tempAnswers.Add(answer);
                }
            }

            return tempAnswers;
        }
    }
}
