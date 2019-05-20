using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class AnswerService : AService<Answer>
    {
        public EduriaContext Context;

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
    }
}
