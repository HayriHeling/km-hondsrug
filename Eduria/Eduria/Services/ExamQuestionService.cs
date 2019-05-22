using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class ExamQuestionService : AService<ExamQuestion>
    {
        public ExamQuestionService(EduriaContext context)
        {
            Context = context;
        }
        public override IEnumerable<ExamQuestion> GetAll()
        {
            return Context.ExamQuestions;
        }

        public override ExamQuestion GetById(int id)
        {
            return Context.ExamQuestions.Find(id);
        }
    }
}
