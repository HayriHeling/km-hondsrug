using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class CreateUserController : Controller
    {
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: CreateUser/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
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
    }
}