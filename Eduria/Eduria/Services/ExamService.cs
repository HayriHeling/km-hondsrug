using EduriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public class ExamService : AService<Exam>
    {
        private EduriaContext Context { get; set; }

        public ExamService(EduriaContext context)
        {
            Context = context;
        }
        public override IEnumerable<Exam> GetAll()
        {
            return Context.Exams;
        }

        public override Exam GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }
    }
}
