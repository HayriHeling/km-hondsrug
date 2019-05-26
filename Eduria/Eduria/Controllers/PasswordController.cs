using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Models;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using WebMatrix.WebData;

namespace Eduria.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult Password()
        {
            return View();
        }
        

        public IActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(string UserName)
        {
            if (ModelState.IsValid)
            {
             
                    
                    string To = UserName, UserID, Password, SMTPPort, Host;
                    string token = Guid.NewGuid().ToString();

                    if (token != null)
                    {
                        //Create URL with above token  

                        var lnkHref = "<a href='" + Url.Action("Reset", "Password", new { code = token }, "http") + "'> Wachtwoord wijzigen</a>";


                        //HTML Template for Send email  

                        string subject = "Verzoek om wachtwoord te wijzigen";

                        string body = "Beste student, Klik op de link om je wachtwoord te resetten." + lnkHref;


                        //Get and set the AppSettings using configuration manager.  

                        EmailManager.AppSettings(out UserID, out Password, out SMTPPort, out Host);

                        //Call send email methods.  

                        EmailManager.SendEmail("info@adindatest3.nl", subject, body, "knevel.adinda@gmail.com", "info@adindatest3.nl", "wzRQ3Gg5mE", "465", "mail.axc.nl");
                        return Content("Er is een mail met een link naar jouw emailadres verzonden.");

                    }
                    else
                    {
                        // If user does not exist or is not confirmed.  
                        return View("Password");

                    }
            }
            return View("Password");
        }

        //public ActionResult ResetPassword(string code, string email)
        //{
        //    ResetPasswordModel model = new ResetPasswordModel
        //    {
        //        Code = code
        //    };
        //    return View(model);
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
        // }
        }

   

}