﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models.ExamLayer;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ResultController : Controller
    {
        private QuestionService QuestionService { get; set; }
        private ExamQuestionService ExamQuestionService { get; set; }

        public ResultController(QuestionService questionService, ExamQuestionService examQuestionService)
        {
            QuestionService = questionService;
            ExamQuestionService = examQuestionService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Question()
        {
            IEnumerable<Question> questions = QuestionService.GetAll();
            IEnumerable<QuestionModel> questionModels = questions.Select(result => new QuestionModel
            {
                QuestionId = result.QuestionId,
                Text = result.Text
            });

            return View(questionModels);
        }

        public IActionResult QuestionResult(int id)
        {
            return Content(ExamQuestionService.GetTotalTimesWrong(id).ToString());
        }
    }
}