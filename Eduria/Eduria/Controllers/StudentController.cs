using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class StudentController : Controller
    {
        private ExamResultService ExamResultService { get; set; }
        private UserService UserService { get; set; }
        private ExamService ExamService { get; set; }

        public StudentController(ExamResultService examResultService, UserService userService, ExamService examService)
        {
            ExamResultService = examResultService;
            UserService = userService;
            ExamService = examService;
        }

        /// <summary>
        /// Default ActionResult
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Show the results from various tests in the database.
        /// </summary>
        /// <returns>An IActionResult that contains an IEnumerable<UserTest> with all its data.</returns>
        public IActionResult TestResults()
        {
            IEnumerable<ExamResult> examResults = ExamResultService.GetAll();
            IEnumerable<User> users = UserService.GetAll();
            IEnumerable<Exam> exams = ExamService.GetAll();

            var result = (from er in examResults
                          join u in users on er.UserId equals u.UserId
                          join e in exams on er.ExamId equals e.ExamId

                          select new UserTestModel
                          {
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              //Category = c.CategoryName,
                              StartedAt = er.StartedAt,
                              FinishedAt = er.FinishedAt,
                              Score = er.Score
                          });

            return View(result);
        }
    }
}
