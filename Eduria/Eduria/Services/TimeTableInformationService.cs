using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models.TimeLineLayer;

namespace Eduria.Services
{
    public class TimeTableInformationService : AService<TimeTableInformation>
    {
        public TimeTableInformationService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<TimeTableInformation> GetAll()
        {
            return Context.TimeTableInformations;
        }

        public override TimeTableInformation GetById(int id)
        {
            return GetAll().First(x => x.TimeTableInformationId == id);
        }

        public IEnumerable<TimeTableInformation> GetAllByUserId(int userId)
        {
            return GetAll().Where(x => x.UserId == userId);
        }

        public IEnumerable<TimeTableInformation> GetAllByTimeTableId(int timeTableId)
        {
            return GetAll().Where(x => x.TimeTableId == timeTableId);
        }

        public IEnumerable<TimeTableInformation> GetAllByTimeTableUserId(int timeTableId, int userId)
        {
            return GetAll().Where(x => x.TimeTableId == timeTableId && x.UserId == userId);
        }
    }
}
