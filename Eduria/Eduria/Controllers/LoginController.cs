﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using EduriaData;
using Eduria.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Eduria.Controllers
{
    public class LoginController : Controller
    {
        private UserService Service { get; set; }

        public LoginController(UserService service)
        {
            Service = service;
        }

        /// <summary>
        /// GET: Login/Login
        /// 
        /// Shows the login page.
        /// </summary>
        /// <returns>The login page.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            if (Request.Cookies["LastLoggedInTime"] != null)
            {
                ViewBag.LLIT = Request.Cookies["LastLoggedInTime"].ToString();
            }

            return View();
        }

        /// <summary>
        /// POST: Login/Login
        /// 
        /// Checks the input from the login page and logs in if input is correct.
        /// </summary>
        /// <param name="user">The user object containing the student number and password from the submitted login form.</param>
        /// <returns>The Login view if input was incorrect, the LoggedIn view if input was correct.</returns>
        [HttpPost]
        public IActionResult Login(User user)
        {
            ClaimsIdentity identity = null;

            User LoggedInUser = Service.GetUserByStudNum(user.StudNum);

            if (LoggedInUser == null)
            {
                ViewBag.Message = "Verkeerde username/password combinatie, probeer het nog eens.";
                return View();
            }

            byte[] hashBytes = Convert.FromBase64String(LoggedInUser.Password);
            Logic hash = new Logic(hashBytes);

            if (!hash.Verify(user.Password))
            {
                ViewBag.Message = "Verkeerde username/password combinatie, probeer het nog eens.";
                return View();
            }

            // Signs a user in with an identity containing a name and a role.
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, LoggedInUser.Firstname + " " + LoggedInUser.Lastname));
            if (LoggedInUser.UserType == 0)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else if (LoggedInUser.UserType == 1)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Teacher"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "Student"));
            }
            identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            Task login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Save user information in session.
            HttpContext.Session.SetInt32("Username", LoggedInUser.StudNum);
            HttpContext.Session.SetInt32("Role", LoggedInUser.UserType);
            HttpContext.Session.SetString("Firstname", LoggedInUser.Firstname);
            HttpContext.Session.SetString("Lastname", LoggedInUser.Lastname);

            // Adds a cookie with the last logged in time.
            Response.Cookies.Append("LastLoggedInTime", DateTime.Now.ToString());

            return RedirectToAction("LoggedIn");
        }

        /// <summary>
        /// GET: Login/LoggedIn
        /// 
        /// Shows the welcome page if the user logged in correctly.
        /// </summary>
        /// <returns>The LoggedIn view if the user is logged in correctly, the Login view if the user isn't logged in.</returns>
        public IActionResult LoggedIn()
        {
            if (HttpContext.Session.GetInt32("Username") == null)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Username = HttpContext.Session.GetInt32("Username");
            ViewBag.Role = HttpContext.Session.GetInt32("Role");
            ViewBag.Firstname = HttpContext.Session.GetString("Firstname");
            ViewBag.Lastname = HttpContext.Session.GetString("Lastname");

            return View();
        }

        /// <summary>
        /// GET: Login/Logout
        /// 
        /// Logs the user out and redirects to the Login page.
        /// </summary>
        /// <returns>The Login view.</returns>
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Task login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        public ActionResult ForgotPassword(string Email)
        {
            Console.WriteLine("Testing!");
            if (ModelState.IsValid)
            {
                string To = Email, UserID, Password, SMTPPort, Host;
                string token = Guid.NewGuid().ToString();

                if (token != null)
                {
                    //Create URL with above token  
                    var lnkHref = "<a href='" + Url.Action("Reset", "Password", new { code = token }, "https") + "'> Wachtwoord wijzigen</a>";

                    //HTML Template for Send email  
                    string subject = "Verzoek om wachtwoord te wijzigen";
                    string body = "Beste student, Klik op de link om je wachtwoord te resetten." + lnkHref;

                    //Get and set the AppSettings using configuration manager.  
                    EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);
                    //return Content("UserID is: " + UserID);
                    //Call send email methods.  
                    EmailManager.SendEmail("info@adindatest3.nl", subject, body, Email, "info@adindatest3.nl", "wzRQ3Gg5mE", "465", "mail.axc.nl");
                    return Content("Er is een mail met een link naar " + Email + " verzonden.");


                    //// Token moet toegevoegd worden aan User
                    //try
                    //{
                    //    User user = new User();
                    //    UserService service = new UserService(this.ControllerContext);
                    //    user = Service.GetUserByEmail(Email);
                    //    user.Token = token;
                    //    Service.Update(user);
                    //    return Content("Er is een mail met een link naar " + Email + " verzonden.");
                    //}
                    //catch
                    //{
                    //    return Content("Er ging iets goed mis..");
                    //}
                }
                else
                {
                    // If user does not exist or is not confirmed.  
                    return View("Password");

                }
            }
            return View("Password");
        }
    }
}