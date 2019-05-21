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
        private UserTestService UserTestService { get; set; }
        private UserService UserService { get; set; }

        //TODO: Add the Service for Test and Category
        //      Then Remove this context
        private EduriaContext Context { get; set; }

        public StudentController(UserTestService userTestService, UserService userService, EduriaContext eduriaContext)
        {
            UserTestService = userTestService;
            UserService = userService;
            Context = eduriaContext;
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
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult TestResults()
        {
            IEnumerable<UserTest> userTests = UserTestService.GetAll();
            IEnumerable<EduriaData.Models.User> users = UserService.GetAll();
            IEnumerable<Test> tests = Context.Tests;
            IEnumerable<Category> categories = Context.Categories;

            var result = (from ut in userTests
                          join u in users on ut.UserId equals u.Id
                          join t in tests on ut.TestId equals t.Id
                          join c in categories on ut.TestId equals c.Id

                          select new UserTestModel
                          {
                              Firstname = u.Firstname,
                              Lastname = u.Lastname,
                              Category = c.CategoryName,
                              StartedAt = ut.StartedAt,
                              FinishedAt = ut.FinishedAt,
                              Score = ut.Score
                          }

                          );

            return View(result);
        }
    }
}
