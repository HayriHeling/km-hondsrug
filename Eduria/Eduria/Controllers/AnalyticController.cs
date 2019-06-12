using Eduria.Models;
using Eduria.Services;
using EduriaData.Models.AnalyticLayer;
using Microsoft.AspNetCore.Http;
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
        /// This is the Index result action.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            IEnumerable<AnalyticHasDefaultModel> analyticDefaultModels = Service.GetAllDataByAnalyticDataId(AnalyticDataId);
            return View(analyticDefaultModels);
        }

        /// <summary>
        /// This is the Method action.
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
            return RedirectToAction("Index", "Analytic");
        }

        /// <summary>
        /// IActionResult that shows the Subject action on the view.
        /// </summary>
        /// <returns>Based on data return the right view.</returns>
        public IActionResult Subject()
        {
            Service.AddSubjectToHasDefaults(AnalyticDataId);
            return View(Service.GetCombinedAnalyticDefaultAndData(AnalyticDataId, (int)AnalyticCategory.Reflectie));         
        }

        /// <summary>
        /// IActionResult that is hit when the form is submitted.
        /// </summary>
        /// <param name="form">IFormCollection that has all the data.</param>
        /// <returns>Redirects the user to the index page.</returns>
        public IActionResult AddScore(IFormCollection form)
        {
            Service.AddDefaultDataScore(form);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: Analytic/Goal
        /// 
        /// Shows the goal view page.
        /// </summary>
        /// <returns>The goal view.</returns>
        public IActionResult Goal()
        {
            return View(Service.GetAnalyticDefaultAndHasDefaultModel(AnalyticDataId, (int)AnalyticCategory.Leerdoel));
        }

        /// <summary>
        /// POST: Analytic/Goal
        /// 
        /// Adds goals to the AnalyticData for the user.
        /// </summary>
        /// <param name="analyticDefaultAndHasDefaultModel"></param>
        /// <returns>The goal view with updated goals.</returns>
        [HttpPost]
        public IActionResult AddGoal(AnalyticDefaultAndHasDefaultModel analyticDefaultAndHasDefaultModel)
        {
            if (analyticDefaultAndHasDefaultModel != null)
            {
                if (analyticDefaultAndHasDefaultModel.AnalyticDefaultModels != null)
                {
                    // Check ischecked true on models met category doel
                    IEnumerable<AnalyticDefaultModel> checkedDefaults = analyticDefaultAndHasDefaultModel.AnalyticDefaultModels.Where(x => x.IsChecked == true);

                    // Check if aantal bestaande > 2, als dat niet geval is, dan check if aangevinkte > 2
                    if (Service.GetAllDefaultsByAnalyticDataIdAndCategoryName(AnalyticDataId, (int)AnalyticCategory.Leerdoel).Count() == 0)
                    {
                        if (checkedDefaults.Count() < 2)
                        {
                            ViewBag.Message = "Je moet minstens twee leerdoelen kiezen.";
                            return RedirectToAction("Goal");
                        }
                    }

                    foreach (var item in checkedDefaults)
                    {
                        // Als item niet bestaat in hasdefaults, dan toevoegen.
                        if (Service.GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(item.AnalyticDefaultId, AnalyticDataId) == null)
                        {
                            Service.AddDataHasDefault(item.AnalyticDefaultId, AnalyticDataId);

                            if (item.AnalyticDefaultOption == (int)DefaultOption.Input || item.AnalyticDefaultOption == (int)DefaultOption.InputScore)
                            {
                                Service.AddInputToAnalyticDefault(item.AnalyticDefaultId, AnalyticDataId, item.Text);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Goal");
        }

        [HttpPost]
        public IActionResult AddGoalScore(AnalyticDefaultAndHasDefaultModel analyticDefaultAndHasDefaultModel)
        {
            if (analyticDefaultAndHasDefaultModel != null)
            {
                if (analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels != null)
                {
                    IEnumerable<AnalyticHasDefaultModel> analyticHasDefaults = analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels.Where(x => x.Score != null);

                    foreach (var item in analyticHasDefaults)
                    {
                        Service.AddScoreToAnalyticDefault(item.AnalyticDefaultId, AnalyticDataId, (int)item.Score);
                    }
                }
            }

            return RedirectToAction("Goal");
        }
    }
}