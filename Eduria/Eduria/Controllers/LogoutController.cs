using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class LogoutController : Controller
    {
        /// <summary>
        /// Logs the user out and redirects to the Login page.
        /// </summary>
        /// <returns>The Login view.</returns>
        public IActionResult Index()
        {
            HttpContext.Session.Clear();
            Task login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Login");
        }
    }
}