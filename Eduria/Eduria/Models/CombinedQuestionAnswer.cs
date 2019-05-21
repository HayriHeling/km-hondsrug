using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Models
{
    public class CombinedQuestionAnswer
    {
        public IQuestion QuestionModel { get; set; }
        public AnswerModel AnswerModel { get; set; }
        public List<AnswerModel> AnswerModels { get; set; }

        /// <summary>
        /// This initiator is for open questions.
        /// </summary>
        /// <param name="question">Question object</param>
        /// <param name="answerModel">Answer object</param>
        public CombinedQuestionAnswer(IQuestion question, AnswerModel answerModel)
        {
            QuestionModel = question;
            AnswerModel = answerModel;
        }

        /// <summary>
        /// This initiator is for multiple-choice questions.
        /// </summary>
        /// <param name="question">Question object</param>
        /// <param name="answerModels">List of possible answers, with one correct answer</param>
        public CombinedQuestionAnswer(IQuestion question, List<AnswerModel> answerModels)
        {
            QuestionModel = question;
            AnswerModels = answerModels;
        }


    }
}
