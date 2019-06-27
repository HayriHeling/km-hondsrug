using System.Net;
using System.Net.Mail;
using EduriaData.Models;
using Microsoft.AspNetCore.Mvc;

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
        public static void SendEmail(string To, Config config, string link)
        {           
            MailMessage mail = new MailMessage();
            mail.To.Add(To);
            mail.From = new MailAddress(config.FromMail);
            mail.Subject = config.Subject;
            mail.Body = (config.Body + link);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = config.Host;
            smtp.Port = config.SMTPPort;
            smtp.Credentials = new NetworkCredential(config.FromMail, config.Password);
            smtp.EnableSsl = false;
            smtp.Send(mail);
        }
    }
}