using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class LecturersController : Controller
    {
        private UserService Service { get; set; }

        public LecturersController(UserService service)
        {
            Service = service;
        }
        /// <summary>
        /// returns the index page with an overview of all lecturers
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            IEnumerable lecturers = Service.GetAllUsersByUserType((int)UserRoles.Docent);
            List<UserModel> lecturerList = new List<UserModel>();
            foreach(User lecturer in lecturers)
            {
                UserModel model = new UserModel()
                {
                    UserId = lecturer.UserId,
                    FirstName = lecturer.Firstname,
                    LastName = lecturer.Lastname
                };
                lecturerList.Add(model);
            }
            ViewBag.lecturers = lecturerList;
            return View();
        }
    }
}