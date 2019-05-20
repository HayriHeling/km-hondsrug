﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class CombinedQuestionController : Controller
    {
        private QuestionService qService;
        private AnswerService aService;
        
        public IActionResult Index()
        {
            return View();
        }

        public void GetAllCombinedQuestions()
        {
            IEnumerable<Question> allQuestions = qService.GetAll();
            IEnumerable<Answer> allAnswers = aService.GetAll();
            List<CombinedQuestionAnswer> allCombinedQuestionAnswers;

            for (int i = 0; i < allQuestions.Count(); i++)
            {
                allCombinedQuestionAnswers.Add(new CombinedQuestionAnswer(new TextQuestionModel(), new AnswerModel()));
            }
        }
    }
}