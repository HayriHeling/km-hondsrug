using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Services;
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

        //public ExamController(ExamService examService, QuestionService questionService, AnswerService answerService)
        //{
        //    this._answerService = answerService;
        //    this._examService = examService;
        //    this._questionService = questionService;
        //}
        public ExamController()
        {

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

        // GET: Exam/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: Exam/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: Exam/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Exam/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}