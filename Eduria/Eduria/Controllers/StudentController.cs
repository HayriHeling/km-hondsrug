using Eduria.Models;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class StudentController : Controller
    {
        private Service<UserTest> Service { get; set; }

        public StudentController(Service<UserTest> service)
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
                return Content("Geen resultaten gevonden.");
            }
            else
            {
                return View(userTestModels);
            }
        }
    }
}