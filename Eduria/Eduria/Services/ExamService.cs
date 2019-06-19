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
                where er.ExamId == id
                select er.ExamId;

            return query
                .DefaultIfEmpty()
                .Count();
        }

        /// <summary>
        /// Get the Exams done in a specific month.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int GetTotalDoneBetweenDate(int id, int month)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                where er.StartedAt.Month == month && er.ExamId == id
                select er.ExamId;

            return query
                .Count();
        }

        /// <summary>
        /// Returns the highest score of an Exam.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetHighestScore(int id)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                where er.ExamId == id
                select er.Score;

            return query
                .DefaultIfEmpty()
                .Max();
        }

        /// <summary>
        /// Returns the lowest score of an Exam.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetLowestScore(int id)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                where er.ExamId == id
                select er.Score;

            return query
                .DefaultIfEmpty()
                .Min();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public double GetAverageScore(int id)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                where er.ExamId == id
                select er.Score;

            return query 
                .Average();
        }

        /// <summary>
        /// Return an list with Users and group them to Firstname
        /// </summary>
        /// <param name="id">ExamId</param>
        /// <returns></returns>
        public IEnumerable<User> GetUsersByExamId(int id)
        {
            var query =
                from e in Context.Exams
                join er in Context.ExamResults on e.ExamId equals er.ExamId
                join ul in Context.UserEQLogs on er.ExamResultId equals ul.ExamResultId
                join u in Context.Users on ul.UserId equals u.UserId
                where e.ExamId == id
                select new User
                {
                    UserId = u.UserId,
                    Firstname = u.Firstname,
                    Lastname = u.Lastname
                };

            return query
                .GroupBy(x => x.Firstname)
                .Select(x => x.First())
                .ToList();
        }
    }
}
