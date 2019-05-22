using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService _examService;
        private QuestionService _questionService;
        private AnswerService _answerService;
        private ExamQuestionService _examQuestionService;

        public ExamController(ExamService examService, QuestionService questionService, ExamQuestionService examQuestionService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
        }

        // GET: Exam
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Exam/Details/5
        public ActionResult Show(int id)
        {
            return View();
        }

        public List<ExamModel> GetExamDataById(int id)
        {
            List<Question> allQuestions = _questionService.GetAll().ToList();
            List<Answer> allAnswers = _answerService.GetAll().ToList();


        }

        public List<AnswerModel> CreateAnswerModels(List<Answer> answers)
        {
            List<AnswerModel> tempAnswerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                tempAnswerModels.Add(new AnswerModel(answer.Id,
                    answer.QuestionId, answer.Text, answer.Correct.Equals(0)));
            }

            return tempAnswerModels;
        }

        //GET: Exam/Create
        public ActionResult Create()
        {
            return View();
        }

        //POST: Exam/Create
       [HttpPost]
       [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                //return RedirectToAction(nameof(Index));
                return null;
            }
            catch
            {
                return View();
            }
        }

        //GET: Exam/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //POST: Exam/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                //return RedirectToAction(nameof(Index));
                return null;
            }
            catch
            {
                return View();
            }
        }

        //GET: Exam/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //POST: Exam/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                //return RedirectToAction(nameof(Index));
                return null;
            }
            catch
            {
                return View();
            }
        }
    }
}