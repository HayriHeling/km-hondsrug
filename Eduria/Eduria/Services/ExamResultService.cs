using Eduria.Services;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;

namespace Eduria
{
    public class ExamResultService : AService<ExamResult>
    {
        public ExamResultService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all the UserTest data from the database.
        /// </summary>
        /// <returns>An list with the data.</returns>
        public override IEnumerable<ExamResult> GetAll()
        {
            return Context.ExamResults;
        }

        /// <summary>
        /// Get an UserTest with its data from the database by the specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The UserTest datamodel.</returns>
        public override ExamResult GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.ExamResultId == id);
        }

        public ExamResult GetExamResultByUserAndExamId(int userId, int examId)
        {
            return Context.ExamResults.First(x => x.UserId == userId && x.ExamId == examId);
        }
    }
}
