using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class CreateUserController : Controller
    {
       
        private UserService Service { get; set; }

        public CreateUserController(UserService service)
        {
            Service = service;
        }

        // GET: CreateUser
        public ActionResult Index()
        {
            return View();
        }

        // GET: CreateUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CreateUser/Create
        public ActionResult Create(int success = 0)
        {
            ViewBag.success = success;
            return View();
        }

        // POST: CreateUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                EduriaData.Models.User dataUser = new EduriaData.Models.User
                {
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Email = user.Email,
                    StudNum = user.UserNum,
                    UserType = (int)user.UserType,
                    ClassId = user.ClassId,
                    Password = user.Password
                };
                Logic hash = new Logic(dataUser.Password);
                byte[] HashBytes = hash.ToArray();
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < HashBytes.Length; i++)
                {
                    builder.Append(HashBytes[i].ToString("x2"));
                }
                dataUser.Password = builder.ToString();
                Service.Add(dataUser);
                return RedirectToAction("Create", new { success = 1 });
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CreateUser/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CreateUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CreateUser/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: CreateUser/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(int id)
        {
            try
            {
                EduriaData.Models.User dataUser = new EduriaData.Models.User
                {
                    Password = user.Password
                };
               
                Service.Update(dataUser);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}