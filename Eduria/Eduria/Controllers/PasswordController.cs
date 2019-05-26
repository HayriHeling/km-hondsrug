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
        

        public IActionResult Reset()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
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

        public ActionResult Edit(string Token, string Password)
        {
            try
            {
                User user = new User();
                user = Service.GetUserByToken(Token);

                Logic hash = new Logic(Password);
                byte[] HashBytes = hash.ToArray();
                user.Password = Convert.ToBase64String(HashBytes);
                Service.Update(user);
                return RedirectToAction("Reset", new { success = 1 });
            }
            catch
            {
                return Content("Token is niet geldig!");
            }
        }


        //public ActionResult ResetPassword(string code, string email)
        //{
        //    ///
        //}

        //[HttpPost]
        //public ActionResult ResetPassword(ResetPasswordModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        bool resetResponse = WebSecurity.ResetPassword(model.ReturnToken, model.Password);
        //        if (resetResponse)
        //        {
        //            ViewBag.Message = "Successfully Changed";
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Something went horribly wrong!";
        //        }
        //    }
        //    return View(model);
        //}
    }



}