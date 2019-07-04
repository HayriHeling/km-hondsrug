using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    [Authorize(Roles = "Student,Teacher")]
    public class TimePeriodsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}