using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class ResetController : Controller
    {
        public IActionResult Reset()
        {
            return View();
        }
    }
}