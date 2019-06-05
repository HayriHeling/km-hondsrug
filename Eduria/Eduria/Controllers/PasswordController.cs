using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using WebMatrix.WebData;
using System.Web;
using EduriaData;

namespace Eduria.Controllers
{
    public class PasswordController : Controller
    {
        
        private UserService Service { get; set; }

        public PasswordController(UserService service)
        {
            Service = service;
        }

        public IActionResult Password()
        {
            return View();
        }
        
        //GET
        public IActionResult Reset(string Token)
        {
            ViewBag.Token = Token;
            return View();
        }

        [HttpPost]
        public IActionResult Reset(string Token, string Password)
        {
            try
            {
                User user = new User();
                user = Service.GetUserByToken(Token);

                if (Password != null)
                {
                    Logic hash = new Logic(Password);
                    byte[] HashBytes = hash.ToArray();
                    user.Password = Convert.ToBase64String(HashBytes);
                    user.Token = null;
                    Service.Update(user);
                }
                return RedirectToAction("Login", "Login");
            }
            catch
            {
                return Content("Token is niet geldig!");
            }
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(include: "Password")] User user)
        {
            if (ModelState.IsValid)
            {
                Service.SetPassword(user);
                return RedirectToAction("Edit");
            }

            return View();
        }
    }



}