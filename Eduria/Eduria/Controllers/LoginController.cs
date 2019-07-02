using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EduriaData.Models;
using Microsoft.AspNetCore.Http;
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
        private ConfigsService _configService { get; set; }
        public LoginController(UserService service, ConfigsService configService)
        {
            Service = service;
            _configService = configService;
        }

        /// <summary>
        /// Shows the login page.
        /// </summary>
        /// <returns>The login page.</returns>
        [HttpGet]
        public IActionResult Index()
        {
            if (Request.Cookies["LastLoggedInTime"] != null)
            {
                ViewBag.LLIT = Request.Cookies["LastLoggedInTime"].ToString();
            }

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        /// <summary>
        /// Checks the input from the login page and logs in if input is correct.
        /// </summary>
        /// <param name="user">The user object containing the student number and password from the submitted login form.</param>
        /// <returns>The Login view if input was incorrect, the LoggedIn view if input was correct.</returns>
        [HttpPost]
        public IActionResult Login(User user)
        {
            ClaimsIdentity identity = null;

            User LoggedInUser = Service.GetUserByStudNum(user.UserNum);

            if (LoggedInUser == null)
            {
                ViewBag.Message = "Verkeerde gebruikersnaam/wachtwoord combinatie, probeer het nog eens.";
                return View();
            }

            byte[] hashBytes = Convert.FromBase64String(LoggedInUser.Password);
            Logic hash = new Logic(hashBytes);

            if (!hash.Verify(user.Password))
            {
                ViewBag.Message = "Verkeerde gebruikersnaam/wachtwoord combinatie, probeer het nog eens.";
                return View();
            }

            // Signs a user in with an identity containing a name and a role.
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, LoggedInUser.Firstname + " " + LoggedInUser.Lastname));
            //Add an NameIdentifier that represents the UserId in the database.
            claims.Add(new Claim(ClaimTypes.NameIdentifier, LoggedInUser.UserId.ToString()));
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
            HttpContext.Session.SetInt32("Username", LoggedInUser.UserNum);
            HttpContext.Session.SetInt32("Role", LoggedInUser.UserType);
            HttpContext.Session.SetString("Firstname", LoggedInUser.Firstname);
            HttpContext.Session.SetString("Lastname", LoggedInUser.Lastname);

            // Adds a cookie with the last logged in time.
            Response.Cookies.Append("LastLoggedInTime", DateTime.Now.ToString());

            return RedirectToAction("Index", "Dashboard");
        }

        /// <summary>
        /// Shows the welcome page if the user logged in correctly.
        /// </summary>
        /// <returns>The LoggedIn view if the user is logged in correctly, the Login view if the user isn't logged in.</returns>
        public IActionResult LoggedIn()
        {
            if (HttpContext.Session.GetInt32("Username") == null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Username = HttpContext.Session.GetInt32("Username");
            ViewBag.Role = HttpContext.Session.GetInt32("Role");
            ViewBag.Firstname = HttpContext.Session.GetString("Firstname");
            ViewBag.Lastname = HttpContext.Session.GetString("Lastname");

            return View();
        }

        public ActionResult ForgotPassword(string Email)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string token = Guid.NewGuid().ToString();

                    if (token != null)
                    {
                        //Create URL with above token  
                        string lnkHref = " <a href='" + Url.Action("Reset", "Password", new { Token = token }, "https") + "'> Wachtwoord wijzigen</a>";
                        Service.SetUserToken(Email, token);

                        //Call send email methods.  
                        EmailManager.SendEmail(Email, _configService.GetNewest(), lnkHref);
                        return RedirectToAction("PasswordReset", new { email = Email });
                    }
                }
                return RedirectToAction("PasswordReset", new { email = Email });
            }
            catch
            {
                return RedirectToAction("PasswordReset", new { email = Email });
            }          
        }

        public IActionResult PasswordReset(string email)
        {
            ViewBag.email = email;
            return View();
        }
    }
}