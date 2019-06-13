using System;
using Eduria.Services;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
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
        
        /// <summary>
        /// Checks if given token is valid and returns view. Returns an empty page with content when token is not valid.
        /// This can only happen when a change password link is used multiple times after already being changed.
        /// </summary>
        /// <param name="Token">The given token to compare with the user.</param>
        /// <returns></returns>
        public IActionResult Reset(string Token)
        {
            User user = Service.GetUserByToken(Token);
            if(user != null)
            {
                ViewBag.Token = Token;
                return View();
            }
            else
            {
                return Content("Token is niet geldig!");
            }                
        }
        /// <summary>
        /// Rechecks if token is valid and changes the password for the user to given password.
        /// </summary>
        /// <param name="Token">The given token to compare with the user.</param>
        /// <param name="Password">The new password to save in the database.</param>
        /// <returns></returns>
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
    }
}