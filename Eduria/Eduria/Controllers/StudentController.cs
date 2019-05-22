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
        private UserExamService UserExamService { get; set; }
        private UserService UserService { get; set; }
        private CategoryService CategoryService { get; set; }
        private ExamService ExamService { get; set; }

        public StudentController(UserExamService userExamService, UserService userService, CategoryService categoryService, ExamService examService)
        {
            UserExamService = userExamService;
            UserService = userService;
            CategoryService = categoryService;
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
            IEnumerable<UserExam> userExams = UserExamService.GetAll();
            IEnumerable<User> users = UserService.GetAll();
            IEnumerable<Exam> exams = ExamService.GetAll();
            IEnumerable<Category> categories = CategoryService.GetAll();

            var result = (from ue in userExams
                          join u in users on ue.UserId equals u.Id
                          join e in exams on ue.ExamId equals e.Id
                          join c in categories on ue.ExamId equals c.Id

                          select new UserTestModel
                          {
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              Category = c.CategoryName,
                              StartedAt = ue.StartedAt,
                              FinishedAt = ue.FinishedAt,
                              Score = ue.Score
                          });

            return View(result);
        }
    }
}
