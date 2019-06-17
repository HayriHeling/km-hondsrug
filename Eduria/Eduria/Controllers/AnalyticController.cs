using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.AnalyticLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Eduria.Controllers
{
    public class AnalyticController : Controller
    {
        private AnalyticDefaultService Service { get; set; }
        private UserService UserService { get; set; }

        public AnalyticController(AnalyticDefaultService service, UserService userService)
        {
            Service = service;
            UserService = userService;
        }

        /// <summary>
        /// This is the Index result action.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student, Teacher")]
        public IActionResult Index()
        {
            int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);
            IEnumerable<AnalyticHasDefaultModel> analyticDefaultModels = Service.GetAllDataByAnalyticDataId(analyticDataId, int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value));
            return View(analyticDefaultModels);
        }

        [Authorize(Roles = "Teacher")]
        public IActionResult Period()
        {
            return View();
        }
        
        public IActionResult AddPeriod(PeriodModel periodModel)
        {
            //First add the PeriodModel to the database.
            Service.AddPeriod(periodModel);
            //Then get that PeriodId by PeriodNum and SchoolYearStart.
            int periodId = Service.GetByPeriodIdByPeriodNumAndStartYear(periodModel.PeriodNum, periodModel.SchoolYearStart);
            //Then get all users by usertype.
            IEnumerable<User> users = UserService.GetAllUsersByUserType((int)(UserRoles.Student));
            //Finally adds for every user an AnalyticData.
            Service.AddAnalyticDataPerUser(users, periodId);
            //Redirect to the Period Action.
            return RedirectToAction("Period");
        }

        /// <summary>
        /// This is the Method action.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student")]
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
        [Authorize(Roles = "Student")]
        public IActionResult AddMethod(int[] methodParam, string textParam)
        {
            int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);
            Service.AddToAnalytic(methodParam, Service.GetAnalyticDataByUserIdAndPeriodAndYear(analyticDataId, 1, 1).AnalyticDataId, textParam);
            return RedirectToAction("Index", "Analytic");
        }

        /// <summary>
        /// IActionResult that shows the Subject action on the view.
        /// </summary>
        /// <returns>Based on data return the right view.</returns>
        [Authorize(Roles = "Student,Teacher")]
        public IActionResult Subject()
        {
            int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);
            Service.AddSubjectToHasDefaults(analyticDataId);
            return View(Service.GetCombinedAnalyticDefaultAndData(analyticDataId, (int)AnalyticCategory.Reflectie));         
        }

        /// <summary>
        /// IActionResult that is hit when the form is submitted.
        /// </summary>
        /// <param name="form">IFormCollection that has all the data.</param>
        /// <returns>Redirects the user to the index page.</returns>
        [Authorize(Roles = "Student")]
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
        [Authorize(Roles = "Student, Teacher")]
        public IActionResult Goal()
        {
            int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);
            return View(Service.GetAnalyticDefaultAndHasDefaultModel(analyticDataId, (int)AnalyticCategory.Leerdoel));
        }

        /// <summary>
        /// POST: Analytic/AddGoal
        /// 
        /// Adds goals to the AnalyticData for the user.
        /// </summary>
        /// <param name="analyticDefaultAndHasDefaultModel"></param>
        /// <returns>The goal view with updated goals.</returns>
        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult AddGoal(AnalyticDefaultAndHasDefaultModel analyticDefaultAndHasDefaultModel)
        {
            if (analyticDefaultAndHasDefaultModel != null)
            {
                if (analyticDefaultAndHasDefaultModel.AnalyticDefaultModels != null)
                {
                    // Check ischecked true on models met category doel
                    IEnumerable<AnalyticDefaultModel> checkedDefaults = analyticDefaultAndHasDefaultModel.AnalyticDefaultModels.Where(x => x.IsChecked == true);
                    int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);

                    // Check if aantal bestaande > 2, als dat niet geval is, dan check if aangevinkte > 2
                    if (Service.GetAllDefaultsByAnalyticDataIdAndCategoryName(analyticDataId, (int)AnalyticCategory.Leerdoel).Count() == 0)
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
                        if (Service.GetDataHasDefaultByAnalyticDefaultIdAndAnalyticDataId(item.AnalyticDefaultId, analyticDataId) == null)
                        {
                            Service.AddDataHasDefault(item.AnalyticDefaultId, analyticDataId);

                            if (item.AnalyticDefaultOption == (int)DefaultOption.Input || item.AnalyticDefaultOption == (int)DefaultOption.InputScore)
                            {
                                Service.AddInputToAnalyticDefault(item.AnalyticDefaultId, analyticDataId, item.Text);
                            }
                        }
                    }
                }
            }

            return RedirectToAction("Goal");
        }

        /// <summary>
        /// POST: Analytic/AddGoalScore
        /// 
        /// Adds a score to the user's AnalyticDefault
        /// </summary>
        /// <param name="analyticDefaultAndHasDefaultModel"></param>
        /// <returns>The goal view with updated scores.</returns>
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult AddGoalScore(AnalyticDefaultAndHasDefaultModel analyticDefaultAndHasDefaultModel)
        {
            if (analyticDefaultAndHasDefaultModel != null)
            {
                if (analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels != null)
                {
                    IEnumerable<AnalyticHasDefaultModel> analyticHasDefaults = analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels.Where(x => x.Score != null);
                    int analyticDataId = Service.GetAnalyticDataIdByUserId(int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value), 1, 1);

                    foreach (var item in analyticHasDefaults)
                    {
                        Service.AddScoreToAnalyticDefault(item.AnalyticDefaultId, analyticDataId, (int)item.Score);
                    }
                }
            }

            return RedirectToAction("Goal");
        }
    }
}