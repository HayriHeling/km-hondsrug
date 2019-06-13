using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class TimeTableService : AService<TimeTable>
    {
        public TimeTableService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<TimeTable> GetAll()
        {
            return Context.TimeTables;
        }

        public override TimeTable GetById(int id)
        {
            return Context.TimeTables.Find(id);
        }
    }
}
