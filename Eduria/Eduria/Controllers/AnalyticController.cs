using Eduria.Models;
using Eduria.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eduria.Controllers
{
    public class AnalyticController : Controller
    {
        private AnalyticDefaultService Service { get; set; }
        private int AnalyticDataId { get; set; }

        public AnalyticController(AnalyticDefaultService service)
        {
            Service = service;
            AnalyticDataId = 1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            IEnumerable<AnalyticHasDefaultModel> analyticDefaultModels = Service.GetAllDataByAnalyticDataId(AnalyticDataId);
            return View(analyticDefaultModels);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Method()
        {
            return View(Service.GetCombinedAnalyticDefaultAndData(1, (int)AnalyticCategory.Werkwijze));
        }

        /// <summary>
        /// Action that is triggerd when a users adds method(s) with an own specified method.
        /// </summary>
        /// <param name="methodParam"></param>
        /// <param name="textParam"></param>
        /// <returns></returns>
        public IActionResult AddMethod(int[] methodParam, string textParam)
        {
            Service.AddToAnalytic(methodParam, Service.GetAnalyticDataByUserIdAndPeriodAndYear(AnalyticDataId, 1, 1).AnalyticDataId, textParam);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Goal()
        {
            return View(Service.GetCombinedAnalyticDefaultAndData(1, (int)AnalyticCategory.Leerdoel));
        }

        [HttpPost]
        public IActionResult Goal(Tuple<IEnumerable<AnalyticHasDefaultModel>, IEnumerable<AnalyticDefaultModel>> modelsTuple)
        {
            // check ischecked true on models met category doel
            IEnumerable<AnalyticDefaultModel> checkedDefaults = modelsTuple.Item2.Where(x => x.IsChecked == true);

            // check if aantal bestaande > 2, als dat niet geval is, dan check if aangevinkte > 2
            if (Service.GetAllDefaultsByAnalyticDataIdAndCategoryName(AnalyticDataId, (int)AnalyticCategory.Leerdoel).Count() == 0)
            {
                if (checkedDefaults.Count() < 2)
                {
                    ViewBag.Message = "Jo je moet twee aanvinken";
                }
            }

            foreach (var item in checkedDefaults)
            {

            }
            // als ze niet bestaan, dan toevoegen

            return View(Service.GetCombinedAnalyticDefaultAndData(1, (int)AnalyticCategory.Leerdoel));
        }
    }
}