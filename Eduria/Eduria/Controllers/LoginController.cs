using System;
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
using Microsoft.AspNetCore.Authorization;

namespace Eduria.Controllers
{
    public class LoginController : Controller
    {
        private UserService Service { get; set; }

        public LoginController(UserService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.LoggedIn = HttpContext.Session.GetString("Firstname");

            if (Request.Cookies["LastLoggedInTime"] != null)
            {
                ViewBag.LLIT = Request.Cookies["LastLoggedInTime"].ToString();
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            User LoggedInUser = Service.GetUserByStudNum(user.StudNum);

            if (LoggedInUser == null)
            {
                ViewBag.Message = "Verkeerd username/password combinatie, probeer het nog eens.";
                return View();
            }

            byte[] hashBytes = Convert.FromBase64String(LoggedInUser.Password);
            Logic hash = new Logic(hashBytes);

            if (!hash.Verify(user.Password))
            {
                ViewBag.Message = "Verkeerd username/password combinatie, probeer het nog eens.";
                return View();
            }

            int role = LoggedInUser.UserType;

            if (role == 0)
            {
                identity = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, LoggedInUser.StudNum.ToString()),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, CookieAuthenticationDefaults.AuthenticationScheme
                );
                isAuthenticated = true;
            }
            else if (role == 1)
            {
                identity = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, LoggedInUser.StudNum.ToString()),
                        new Claim(ClaimTypes.Role, "Teacher")
                    }, CookieAuthenticationDefaults.AuthenticationScheme
                );
                isAuthenticated = true;
            }
            else
            {
                identity = new ClaimsIdentity(
                    new[] {
                        new Claim(ClaimTypes.Name, LoggedInUser.StudNum.ToString()),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, CookieAuthenticationDefaults.AuthenticationScheme
                );
                isAuthenticated = true;
            }

            if (isAuthenticated)
            {
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                Task login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Save user information in session
                HttpContext.Session.SetInt32("Username", LoggedInUser.StudNum);
                HttpContext.Session.SetInt32("Role", LoggedInUser.UserType);
                HttpContext.Session.SetString("Firstname", LoggedInUser.Firstname);
                HttpContext.Session.SetString("Lastname", LoggedInUser.Lastname);

                Response.Cookies.Append("LastLoggedInTime", DateTime.Now.ToString());
            }

            return RedirectToAction("LoggedIn");
        }

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

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            Task login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }
    }
}