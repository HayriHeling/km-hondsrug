using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}