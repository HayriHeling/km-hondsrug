using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData;
using EduriaData.Models;
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
        /// <summary>
        /// Shows list of all users.
        /// </summary>
        /// <returns></returns>
        // GET: CreateUser
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows details of a user based on id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: CreateUser/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        /// <summary>
        /// Shows Add user form in cshtml. optional parameter gives succes message if 1.
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        // GET: CreateUser/Create
        public ActionResult Create(string msg = "", int success = 0)
        {
            ViewBag.msg = msg;
            ViewBag.success = success;
            return View();
        }

        /// <summary>
        /// Creates user from form information given and saves it in database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        // POST: CreateUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel user)
        {
            
            if(Service.GetUserByStudNum(user.UserNum) != null)
            {
                return RedirectToAction("Create", new { msg = "De identificatie code " + Service.GetUserByStudNum(user.UserNum).UserNum + " bestaat al!", success = 0 });
            }
            if(Service.GetUserByEmail(user.Email) != null)
            {
                return RedirectToAction("Create", new { msg = "Het email adres " + Service.GetUserByEmail(user.Email).Email + " is al in gebruik!", success = 0 });
            }
            try
            {
                User dataUser = new User
                {
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Email = user.Email,
                    UserNum = user.UserNum,
                    UserType = (int)user.UserType,
                    ClassId = user.ClassId,
                    Password = user.Password
                };
                Logic hash = new Logic(dataUser.Password);
                byte[] HashBytes = hash.ToArray();
                dataUser.Password = Convert.ToBase64String(HashBytes);
                Service.Add(dataUser);
                return RedirectToAction("Create", new { msg = "Gebruiker is succesvol toegevoegd!", success = 1});
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Shows view to change user information based on id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: CreateUser/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        /// <summary>
        /// changes given changes to user and saves it in database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Shows view where a user can be deleted.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: CreateUser/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        /// <summary>
        /// Deletes a user from database.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
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
    }
}