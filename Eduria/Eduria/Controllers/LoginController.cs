using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class LoginController : Controller
    {
        EduriaContext EC = null;
        public LoginController(EduriaContext eC)
        {
            EC = eC;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Authorize(EduriaData.Models.User userModel)
        {
            return View();
        }
    }
}