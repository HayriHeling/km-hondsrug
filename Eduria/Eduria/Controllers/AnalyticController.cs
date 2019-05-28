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
            IEnumerable<AnalyticHasDefaultModel> analyticDefaultModels = Service.GetAllDataByAnalyticDataId(1);

            return View(analyticDefaultModels);
        }

        public IActionResult AddMethod()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Goal()
        {

            IEnumerable<AnalyticHasDefaultModel> analyticDefaultModels = Service.GetAllDefaultsByAnalyticDataIdAndCategoryName(1, "Doel");

            return View(analyticDefaultModels);
        }
    }
}