﻿using System.Collections;
using System.Collections.Generic;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Authorization;
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
                lecturerList.Add(ConvertToUserModel(lecturer));
            }
            ViewBag.lecturers = lecturerList;
            return View();
        }

        public UserModel ConvertToUserModel(User user)
        {
            return new UserModel
            {
                UserId = user.UserId,
                FirstName = user.Firstname,
                LastName = user.Lastname
            };
        }
    }
}