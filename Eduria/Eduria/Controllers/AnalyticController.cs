using Eduria.Models;
using Eduria.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class AnalyticController : Controller
    {
        private AnalyticDefaultService Service { get; set; }
        public AnalyticController(AnalyticDefaultService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            IEnumerable<AnalyticDefaultModel> analyticDefaultModels = Service.GetAllDataByAnalyticDataId(1);
            return View(analyticDefaultModels);
        }

        public IActionResult Method()
        {
            IEnumerable<AnalyticMethodModel> analyticMethodModels = Service.GetAllAnalyticMethods();
            return View(analyticMethodModels);
        }

        public IActionResult AddMethod()
        {
            return RedirectToAction("Index");
        }
    }
}