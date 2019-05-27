using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class QuestionHasAnswerTService : AService<QuestionHasAnswerT>
    {
        public QuestionHasAnswerTService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<QuestionHasAnswerT> GetAll()
        {
            return Context.QuestionHasAnswerTs;
        }

        public override QuestionHasAnswerT GetById(int id)
        {
            return Context.QuestionHasAnswerTs.Find(id);
        }

        public QuestionHasAnswerT GetByQuestionId(int questionId)
        {
            return Context.QuestionHasAnswerTs.First(x => x.QuestionId == questionId);
        }
    }
}
