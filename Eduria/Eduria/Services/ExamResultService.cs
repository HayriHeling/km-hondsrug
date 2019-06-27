using Eduria.Services;
using System.Collections.Generic;
using System.Linq;
using EduriaData.Models.ExamLayer;
using System;
using Eduria.Models;

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
            return Context.ExamResults.FirstOrDefault(x => x.UserId == userId && x.ExamId == examId);
        }

        public ExamResult GetExamResultByUserAndStartDate(int userId, DateTime dateStarted)
        {
            return Context.ExamResults.First(x => x.UserId == userId && x.StartedAt == dateStarted);
        }

        public int GetExamResultIdByExamResult(ExamResult examResult)
        {
            return Context.ExamResults.Find(examResult).ExamResultId;
        }
        public IEnumerable<ExamResult> GetExamResultByUserId(int userId)
        {
            return GetAll().Where(x => x.UserId == userId);
        }
        public IEnumerable<DataStudentResultModel> GetDataStudentResultByUserId(int userId)
        {
            var query =
                from er in Context.ExamResults
                join e in Context.Exams on er.ExamId equals e.ExamId
                join u in Context.Users on er.UserId equals u.UserId
                where er.UserId == userId
                select new DataStudentResultModel
                {
                    ExamId = e.ExamId,
                    ExamResultId = er.ExamResultId,
                    ExamName = e.Name,
                    ExamDescription = e.Description,
                    UserId = u.UserId,
                    StartedAt = er.StartedAt,
                    FinishedAt = er.FinishedAt,
                    Score = er.Score
                };

            return query;
        }
    }
}