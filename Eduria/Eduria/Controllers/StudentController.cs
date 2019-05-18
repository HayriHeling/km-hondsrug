using Eduria.Models;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Eduria.Controllers
{
    public class StudentController : Controller
    {
        private EduriaContext Context { get; set; }

        public StudentController(EduriaContext context)
        {
            Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestResults()
        {
            UserTestModel userTest = new UserTestModel
            {
                Id = 1,
                TestId = new Test
                {
                    Id = 1,
                    Category = new Category
                    {
                        Id = 1,
                        CategoryName = "Romeinen"
                    }
                },
                UserId = new User
                {
                    Id = "1",
                    Firstname = "Hayri",
                    Lastname = "Heling",
                },
                StartedAt = DateTime.Now,
                FinishedAt = DateTime.Now.AddHours(2.00),
                Score = 40

            };

            return View(userTest);
        }
    }
}