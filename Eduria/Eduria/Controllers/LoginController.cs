using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using EduriaData;

namespace Eduria.Controllers
{
    public class LoginController : Controller
    {
        EduriaContext EC = null;
        public LoginController(EduriaContext eC)
        {
            EC = eC;
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
            byte[] hashBytes = Encoding.ASCII.GetBytes(x.Password);
            Logic hash = new Logic(hashBytes);
            if (hash.Verify(user.Password)) { }

            User LoggedInUser = EC.Users.Where(x => x.StudNum == user.StudNum && x.Password == user.Password).FirstOrDefault();

            if (LoggedInUser == null)
            {
                ViewBag.Message = "Verkeerd username/password combinatie, probeer het nog eens.";
                return View();
            }

            // Save user information in session
            HttpContext.Session.SetInt32("Username", LoggedInUser.StudNum);
            HttpContext.Session.SetInt32("Role", LoggedInUser.UserType);
            HttpContext.Session.SetString("Firstname", LoggedInUser.Firstname);
            HttpContext.Session.SetString("Lastname", LoggedInUser.Lastname);
            
            Response.Cookies.Append("LastLoggedInTime", DateTime.Now.ToString());

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

            return RedirectToAction("Login");
        }

        //[HttpPost]
        //public IActionResult Authorize(EduriaData.Models.User userModel)
        //{
        //    return View();
        //}
    }
}