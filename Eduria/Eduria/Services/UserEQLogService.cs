using EduriaData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduria.Services
{
    public class UserTQLogService : AService<UserEQLog>
    {
        public UserTQLogService(EduriaContext context)
        {
            Context = context;
        }

        public override IEnumerable<UserEQLog> GetAll()
        {
            return Context.UserEQLogs;
        }

        public override UserEQLog GetById(int id)
        {
            return Context.UserEQLogs.Find(id);
        }

        public void AddLogs(int userId, int examId, IDictionary<int, int> log)
        {
            foreach (KeyValuePair<int, int> logItem in log)
            {
                Add(new UserEQLog
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
