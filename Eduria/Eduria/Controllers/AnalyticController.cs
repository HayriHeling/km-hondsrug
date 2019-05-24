using Eduria.Models;
using Eduria.Services;
using EduriaData.Models.AnalyticLayer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class AnalyticController : Controller
    {
        private AnalyticMethodService Service { get; set; }
        public AnalyticController(AnalyticMethodService service)
        {
            Service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Method()
        {
            IEnumerable<AnalyticMethod> analyticMethods = Service.GetAll();
            IEnumerable<AnalyticMethodModel> analyticMethodModels = analyticMethods.Select(result => new AnalyticMethodModel
            {
                Id = result.AnalyticMethodId,
                Name = result.AnalyticMethodName
            });

            return View(analyticMethodModels);
        }
    }
}