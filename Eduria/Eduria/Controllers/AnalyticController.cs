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
            IEnumerable<AnalyticDefaultModel> analyticDefaultModels = Service.GetAllAnalyticMethods();
            return View(analyticDefaultModels);
        }

        public IActionResult AddMethod(int[] methodParam)
        {
            if (methodParam.Length != 0)
            {
                foreach (var id in methodParam)
                {
                    Service.AddDataHasDefault(id, 1);
                }
            }
            return RedirectToAction("Index");
        public IActionResult Goal()
        {
            return View(Service.GetCombinedAnalyticDefaultAndData(1, "Doel"));
        }
    }
}