using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class ExamQuestionService : AService<TestQuestion>
    {
        public ExamQuestionService(EduriaContext context)
        {
            Context = context;
        }
        public override IEnumerable<TestQuestion> GetAll()
        {
            return Context.TestQuestions;
        }

        public override TestQuestion GetById(int id)
        {
            return Context.TestQuestions.Find(id);
        }
    }
}
