﻿using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
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
    }
}
