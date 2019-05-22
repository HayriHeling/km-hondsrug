using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class QuestionService : AService<Question>
    {
        public QuestionService(EduriaContext context)
        {
            this.Context = context;
        }

        public override IEnumerable<Question> GetAll()
        {
            return Context.Questions;
        }

        public override Question GetById(int id)
        {
            return Context.Questions.Find(id);
        }

        public List<Question> GetQuestionsByExamId(int examId)
        {
            return null;
        }
    }
}
