﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EduriaData.Models;

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
    }
}
