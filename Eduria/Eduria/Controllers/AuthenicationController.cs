using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class AuthenicationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}