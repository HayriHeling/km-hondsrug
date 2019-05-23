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
    /// <summary>
    /// This is deprecated.
    /// </summary>
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
            return View(GetAllCombinedQuestions());
        }

        // TODO: line 40: Change int to bool)
        public List<CombinedQuestionAnswer> GetAllCombinedQuestions()
        {
            List<Question> allQuestions = qService.GetAll().ToList();
            List<Answer> allAnswers = aService.GetAll().ToList();

            List<AnswerModel> tempAnswerModels = CreateAnswerModels(allAnswers);

            List<CombinedQuestionAnswer> allCombinedQuestionAnswers = new List<CombinedQuestionAnswer>();

            for (int i = 0; i < allQuestions.Count(); i++)
            {
                List<AnswerModel> currentAnswerModels = tempAnswerModels.FindAll(o => o.QuestionId == allQuestions[i].Id);
                
                allCombinedQuestionAnswers.Add(new CombinedQuestionAnswer(
                    new QuestionModel()
                    {
                        QuestionId = allQuestions[i].Id,
                        Text = allQuestions[i].Text
                    }, 
                    currentAnswerModels));
            }
            return allCombinedQuestionAnswers;
        }

        public List<AnswerModel> CreateAnswerModels(List<Answer> answers)
        {
            List<AnswerModel> tempAnswerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                tempAnswerModels.Add(new AnswerModel()
                {
                    AnswerId = answer.Id,
                    QuestionId = answer.QuestionId,
                    Text = answer.Text,
                    CorrectAnswer = answer.Correct.Equals(0)
                });
            }

            return tempAnswerModels;
        }


    }
}