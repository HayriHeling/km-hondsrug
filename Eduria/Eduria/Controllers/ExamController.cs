using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;

namespace Eduria.Controllers
{
    public class ExamController : Controller
    {
        private ExamService _examService;
        private QuestionService _questionService;
        private AnswerService _answerService;
        private ExamQuestionService _examQuestionService;

        public ExamController(ExamService examService, QuestionService questionService, AnswerService answerService, ExamQuestionService examQuestionService)
        {
            this._examQuestionService = examQuestionService;
            this._examService = examService;
            this._questionService = questionService;
            this._answerService = answerService;
        }

        // GET: Exam
        //public ActionResult Index()
        //{
        //    return View();
        //}

        // GET: Exam/Details/5
        public IActionResult Show(int id=1)
        {
            //return View(GetExamDataById(id));
            return View(GetExamModelByExamId(id));
        }

        public ExamModel GetExamModelByExamId(int id)
        {
            IEnumerable<ExamQuestion> tempExamQuestions = _examQuestionService.GetAllQuestionIdsAsList(id);
            IEnumerable<Question> tempQuestions = _questionService.GetQuestionsByExamQuestionList(tempExamQuestions);
            IEnumerable<Answer> tempAnswers = _answerService.GetAnswersByQuestionsList(tempQuestions);

            return new ExamModel()
            {
                AnswerModels = CreateAnswerModels(tempAnswers.ToList()),
                Category = "",
                Description = "",
                ExamId = id,
                Name = "None",
                QuestionModels = CreateQuestionModelsList(tempQuestions.ToList())
            };
        }

        public ExamModel GetExamDataById(int id)
        {
            List<Question> allQuestions = _questionService.GetAll().ToList();
            List<Answer> allAnswers = _answerService.GetAll().ToList();

            List<AnswerModel> answerModels = CreateAnswerModels(allAnswers);
            List<QuestionModel> questionModels = CreateQuestionModelsList(allQuestions);

            return new ExamModel()
            {
                AnswerModels = answerModels,
                Category = "",
                Description = "",
                ExamId = 0,
                Name = "None",
                QuestionModels = questionModels
            };
        }

        public List<QuestionModel> CreateQuestionModelsList(List<Question> questions)
        {
            List<QuestionModel> outputList = new List<QuestionModel>();
            foreach (Question question in questions)
            {
                outputList.Add(new QuestionModel()
                {
                    Category = question.CategoryId.ToString(),
                    MediaLink = question.MediaLink,
                    MediaType = 0,
                    QuestionId = question.Id,
                    Text = question.Text
                });
            }

            return outputList;
        }

        public List<AnswerModel> CreateAnswerModels(List<Answer> answers)
        {
            List<AnswerModel> tempAnswerModels = new List<AnswerModel>();
            foreach (Answer answer in answers)
            {
                tempAnswerModels.Add(new AnswerModel()
                {
                    AnswerId = answer.Id,
                    CorrectAnswer = answer.Correct.Equals(0),
                    QuestionId = answer.QuestionId,
                    Text = answer.Text
                });
            }

            return tempAnswerModels;
        }

        //GET: Exam/Create
        public ActionResult Create()
        {
            return View();
        }
        // GET: Exam/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exam/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Create");
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