using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models.TimeLineLayer;

namespace Eduria.Services
{
    public class TimeTableInfoMediaSrcService : AService<TimeTableInfoHasMediaSrc>
    {
        public TimeTableInfoMediaSrcService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<TimeTableInfoHasMediaSrc> GetAll()
        {
            return Context.TimeTableInfoHasMediaSrcs;
        }

        public override TimeTableInfoHasMediaSrc GetById(int id)
        {
            return GetAll().First(x => x.TimeTableInfoHasMediaSrcId == id);
        }

        public IEnumerable<TimeTableInfoHasMediaSrc> GetAllByTimeTableInfoId(int id)
        {
            return GetAll().Where(x => x.TimeTableInformationId == id);
        }
    }
}
