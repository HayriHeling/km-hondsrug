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

        public CombinedQuestionAnswer(IQuestion question, AnswerModel answerModel)
        {
            question = new TextQuestionModel();
            answerModel = new AnswerModel();
        }


    }
}
