using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class CombinedQuestionController : Controller
    {
        private QuestionService qService;
        private AnswerService aService;

        public CombinedQuestionController(QuestionService questionService, AnswerService answerService)
        {
            qService = questionService;
            aService = answerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void GetAllCombinedQuestions()
        {
            IEnumerable<Question> allQuestions = qService.GetAll();
            IEnumerable<Answer> allAnswers = aService.GetAll();
        }
    }
}