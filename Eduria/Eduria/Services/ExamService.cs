using EduriaData.Models;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class ExamService : AService<Exam>
    {
        public ExamService(EduriaContext context)
        {
            Context = context;
        }

        /// <summary>
        /// Get all exams from the database.
        /// </summary>
        /// <returns>A List with all exams.</returns>
        public override IEnumerable<Exam> GetAll()
        {
            return Context.Exams;
        }

        /// <summary>
        /// Get an specific exam. 
        /// </summary>
        /// <param name="id">The id from the exam.</param>
        /// <returns>The specific category.</returns>
        public override Exam GetById(int id)
        {
            return GetAll().FirstOrDefault(x => x.ExamId == id);
        }

        public Exam GetByName(string name)
        {
            return GetAll().FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Returns a total done of an specific exam.
        /// </summary>
        /// <returns></returns>
        public int GetTotalDone(int id)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                select er.ExamId;

            return query
                .DefaultIfEmpty()
                .Count();
        }
    }
}
