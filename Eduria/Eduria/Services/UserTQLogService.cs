using EduriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public class UserTQLogService : AService<UserTQLog>
    {
        public UserTQLogService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<UserTQLog> GetAll()
        {
            return Context.UserTQLogs;
        }

        public override UserTQLog GetById(int id)
        {
            return Context.UserTQLogs.Find(id);
        }

        public void AddLogs(int userId, int examId, IDictionary<int, int> log)
        {
            foreach (KeyValuePair<int, int> logItem in log)
            {
                Add(new UserTQLog
                {
                    UserId = userId,
                    ExamId = examId,
                    QuestionId = logItem.Key,
                    TimesWrong = logItem.Value
                });
            }
        }
    }
}
