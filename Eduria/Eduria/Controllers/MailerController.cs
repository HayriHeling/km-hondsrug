using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols;

namespace Eduria.Controllers
{
    public class MailerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

    public class EmailManager
    {
        public static void SendEmail(string To, Config config)
        {           
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(config.FromMail);
            mail.Subject = config.Subject;
            mail.Body = config.Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = config.Host;
            smtp.Port = config.SMTPPort;
            smtp.Credentials = new NetworkCredential(config.FromMail, config.Password);
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }

        public static void SendEmail(string From, string Subject, string Body, string To, string UserID, string Password, string SMTPPort, string Host)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(From);
            mail.Subject = Subject;
            mail.Body = Body;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mail.adindatest3.nl";
            smtp.Port = 587;
            smtp.Credentials = new NetworkCredential(UserID, Password);
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }
    }
}