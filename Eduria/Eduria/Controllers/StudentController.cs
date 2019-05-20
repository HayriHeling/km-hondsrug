using Eduria.Models;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class StudentController : Controller
    {
        private UserTestService Service { get; set; }

        public StudentController(UserTestService service)
        {
            Service = service;
        }

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
            IEnumerable<UserTest> userTests = Service.GetAll();
            IEnumerable<UserTestModel> userTestModels = userTests.Select(result => new UserTestModel
            {
                Test = result.Test,
                User = result.User,
                StartedAt = result.StartedAt,
                FinishedAt = result.FinishedAt,
                Score = result.Score     
            });

            if(userTestModels.Count() > 0)
            {
                return View(userTestModels);
            }
            else
            {
                return Content("Geen resultaten gevonden.");
            }
        }
    }
}