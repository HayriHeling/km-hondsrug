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

        //public IEnumerable<Question> GetAllQuestionsAndTotalTimesWrong()
        //{
        //    var query =
        //        from q in Context.Questions
        //        join eq in Context.ExamQuestions on q.QuestionId equals eq.QuestionId
        //        join ul in Context.UserEQLogs on eq.ExamHasQuestionId equals ul.ExamHasQuestionId
        //        group q by q.QuestionId into groupedQuestions
        //        select new Question
        //        {
        //            QuestionId = groupedQuestions.Key,
        //            T
        //        }

        //}
    }
}


//SELECT SUM(ul.TimesWrong) AS TotalTimesWrong
//FROM Questions q
//LEFT JOIN ExamQuestions eq ON q.QuestionId = eq.QuestionId
//LEFT JOIN UserEQLogs ul ON eq.ExamHasQuestionId = ul.ExamHasQuestionId
//WHERE ul.UserEQLogId IS NOT NULL
//GROUP BY q.QuestionId
