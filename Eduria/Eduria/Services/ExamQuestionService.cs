using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;

namespace Eduria.Services
{
    public class ExamQuestionService : AService<ExamQuestion>
    {
        public ExamQuestionService(EduriaContext context)
        {
            Context = context;
        }
        public override IEnumerable<ExamQuestion> GetAll()
        {
            return Context.ExamQuestions;
        }

        public override ExamQuestion GetById(int id)
        {
            return Context.ExamQuestions.Find(id);
        }

        /// <summary>
        /// Gets all Questions belonging to the examId.
        /// </summary>
        /// <param name="examId">The exam-id the questions belong to</param>
        /// <returns>List of ExamQuestion-models</returns>
        public IEnumerable<ExamQuestion> GetAllQuestionIdsAsList(int examId)
        {
            return Context.ExamQuestions.Where(x => x.ExamId == examId);
        }

        public ExamQuestion GetExamQuestionByQuestionIdExamId(int questionId, int examId)
        {
            return Context.ExamQuestions.First(x => x.QuestionId == questionId && x.ExamId == examId);
        }

        /// <summary>
        /// Get a sum of the record TimesWrong per Question.
        /// </summary>
        /// <param name="id">The QuestionId</param>
        /// <returns>TotalTimesWrong.</returns>
        public int GetTotalTimesWrong(int id)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where q.QuestionId == id
                select ul.TimesWrong;
                 
            return query
                .DefaultIfEmpty(0)
                .Sum();
        }

        public int GetTotalTimesWrong(int questionId, int examId)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where q.QuestionId == questionId && eq.ExamId == examId
                select ul.TimesWrong;

            return query
                .DefaultIfEmpty()
                .Sum();
        }

        /// <summary>
        /// Get the total times good.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetTotalTimesGood(int id)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where q.QuestionId == id
                select ul.CorrectAnswered;

            return query
                .DefaultIfEmpty(0)
                .Sum();
        }

        public int GetTotalTimesGood(int questionId, int examId)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where q.QuestionId == questionId && eq.ExamId == examId
                select ul.CorrectAnswered;

            return query
                .DefaultIfEmpty()
                .Sum();
        }

        /// <summary>
        /// Get the name of the question.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetQuestionName(int id)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where q.QuestionId == id
                select q.Text;

            return query
                .First()
                .ToString();
        }

        /// <summary>
        /// Get all the questions by the exam id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IEnumerable<Question> GetQuestionsByExamId(int id)
        {
            var query =
                from q in Context.Questions
                join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
                join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
                where eq.ExamId == id
                select new Question
                {
                    QuestionId = q.QuestionId,
                    Text = q.Text
                };

            return query
                .GroupBy(x => x.QuestionId)
                .Select(x => x.First())
                .ToList();
        }
    }
}