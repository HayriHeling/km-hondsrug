using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

namespace Eduria.Services
{
    public class ExamService : AService<Test>
    {
        public ExamService(EduriaContext context)
        {
            this.Context = context;
        }

        public override IEnumerable<Test> GetAll()
        {
            return Context.Tests;
        }

        public override Test GetById(int id)
        {
            return Context.Tests.Find(id);
        }
    }
}
