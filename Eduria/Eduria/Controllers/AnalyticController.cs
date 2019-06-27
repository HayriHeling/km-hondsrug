using Eduria.Models;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.AnalyticLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        [HttpGet]
        public IActionResult Index()
        {
            List<EduriaData.Models.User> allUsersByUserType = UserService.GetAllUsersByUserType((int)UserRoles.Student).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in allUsersByUserType)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = item.UserId.ToString() + ": " + item.Firstname + " " + item.Lastname,
                    Value = item.UserId.ToString()
                });
            }
            SelectList selectList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.userList = selectList;

            AnalyticDataAndUsersModel analyticDataAndUsers = new AnalyticDataAndUsersModel
            {
                AnalyticDataModels = Service.GetAllAnalyticDatasByUserId(UserService.GetLoggedInUserId(User)).ToList(),
                UserModels = allUsersByUserType
            };
            return View(analyticDataAndUsers);
        }

        [HttpPost]
        public IActionResult Index(List<User> userModels)
        {
            List<User> allUsersByUserType = UserService.GetAllUsersByUserType((int)UserRoles.Student).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in allUsersByUserType)
            {
                selectListItems.Add(new SelectListItem
                {
                    Text = item.UserId.ToString() + ": " + item.Firstname + " " + item.Lastname,
                    Value = item.UserId.ToString()
                });
            }
            SelectList selectList = new SelectList(selectListItems, "Value", "Text");
            ViewBag.userList = selectList;

            AnalyticDataAndUsersModel analyticDataAndUsers = new AnalyticDataAndUsersModel
            {
                AnalyticDataModels = Service.GetAllAnalyticDatasByUserId(userModels.First().UserId).ToList(),
                UserModels = allUsersByUserType
            };
            return View(analyticDataAndUsers);
        }

        [HttpGet]
        public IActionResult Show()
        {
            AnalyticDataModel analyticData = new AnalyticDataModel
            {
                PeriodNum = (int)HttpContext.Session.GetInt32("Period"),
                SchoolYearStart = (int)HttpContext.Session.GetInt32("SchoolYear")
            };

            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            return View(Service.GetAnalyticDataIdAndHasDefaults(analyticDataId));
        }

        [HttpPost]
        public IActionResult Show(AnalyticDataAndUsersModel analyticDataAndUsersModel)
        {
            AnalyticDataModel analyticData = analyticDataAndUsersModel.AnalyticDataModels.First();
            int analyticDataId = User.IsInRole("Teacher") ? analyticData.AnalyticDataId : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetInt32("AnalyticId", analyticData.AnalyticDataId);
            HttpContext.Session.SetInt32("Period", analyticData.PeriodNum);
            HttpContext.Session.SetInt32("SchoolYear", analyticData.SchoolYearStart);

            return View(Service.GetAnalyticDataIdAndHasDefaults(analyticDataId));
        }

        [HttpGet]
        public IActionResult Method()
        {
            AnalyticDataModel analyticData = new AnalyticDataModel
            {
                PeriodNum = (int)HttpContext.Session.GetInt32("Period"),
                SchoolYearStart = (int)HttpContext.Session.GetInt32("SchoolYear")
            };

            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            return View(Service.GetCombinedAnalyticDefaultAndData(analyticDataId, (int)AnalyticCategory.Werkwijze));
        }

        /// <summary>
        /// This is the Method action.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Student,Teacher")]
        [HttpPost]
        public IActionResult Method(AnalyticDataModel analyticData)
        {
            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            return View(Service.GetCombinedAnalyticDefaultAndData(analyticDataId, (int)AnalyticCategory.Werkwijze));
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
            AnalyticDataModel analyticData = new AnalyticDataModel
            {
                PeriodNum = (int)HttpContext.Session.GetInt32("Period"),
                SchoolYearStart = (int)HttpContext.Session.GetInt32("SchoolYear")
            };

            int analyticDataId = Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            Service.AddToAnalytic(methodParam, analyticDataId, textParam);
            return RedirectToAction("Method");
        }

        [HttpGet]
        public IActionResult Subject()
        {
            AnalyticDataModel analyticData = new AnalyticDataModel
            {
                PeriodNum = (int)HttpContext.Session.GetInt32("Period"),
                SchoolYearStart = (int)HttpContext.Session.GetInt32("SchoolYear")
            };

            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            return View(Service.GetCombinedAnalyticDefaultAndData(analyticDataId, (int)AnalyticCategory.Reflectie));
        }

        /// <summary>
        /// IActionResult that shows the Subject action on the view.
        /// </summary>
        /// <returns>Based on data return the right view.</returns>
        [Authorize(Roles = "Student,Teacher")]
        [HttpPost]
        public IActionResult Subject(AnalyticDataModel analyticData)
        {
            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            Service.AddSubjectToHasDefaults(analyticDataId);
            return View(Service.GetCombinedAnalyticDefaultAndData(analyticDataId, (int)AnalyticCategory.Reflectie));         
        }

        /// <summary>
        /// IActionResult that is hit when the form is submitted.
        /// </summary>
        /// <param name="form">IFormCollection that has all the data.</param>
        /// <returns>Redirects the user to the index page.</returns>
        [Authorize(Roles = "Student")]
        [HttpPost]
        public IActionResult AddScore(IFormCollection form)
        {
            Service.AddDefaultDataScore(form);
            return RedirectToAction("Subject");
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public IActionResult AddMethodScore(IFormCollection form)
        {
            Service.AddDefaultDataScore(form);
            return RedirectToAction("Method");
        }

        [HttpGet]
        public IActionResult Goal()
        {
            AnalyticDataModel analyticData = new AnalyticDataModel
            {
                PeriodNum = (int)HttpContext.Session.GetInt32("Period"),
                SchoolYearStart = (int)HttpContext.Session.GetInt32("SchoolYear")
            };

            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1)
            {
                return RedirectToAction("Index");
            }

            return View(Service.GetAnalyticDefaultAndHasDefaultModel(analyticDataId, (int)AnalyticCategory.Leerdoel));
        }

        /// <summary>
        /// POST: Analytic/Goal
        /// 
        /// Shows the goal view page.
        /// </summary>
        /// <returns>The goal view.</returns>
        [Authorize(Roles = "Student, Teacher")]
        [HttpPost]
        public IActionResult Goal(AnalyticDataModel analyticData)
        {
            int analyticDataId = User.IsInRole("Teacher") ? (int)HttpContext.Session.GetInt32("AnalyticId") : Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

            if (analyticDataId == -1) {
                return RedirectToAction("Index");
            }

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
            AnalyticDataModel analyticData = analyticDefaultAndHasDefaultModel.AnalyticData;

            if (analyticDefaultAndHasDefaultModel.AnalyticDefaultModels != null)
            {
                // Check ischecked true on models met category doel
                IEnumerable<AnalyticDefaultModel> checkedDefaults = analyticDefaultAndHasDefaultModel.AnalyticDefaultModels.Where(x => x.IsChecked == true);

                int analyticDataId = Service.GetAnalyticDataIdByUserIdAndPeriodAndYear(UserService.GetLoggedInUserId(User), analyticData.PeriodNum, analyticData.SchoolYearStart);

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
            if (analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels.Any())
            {
                IEnumerable<AnalyticHasDefaultModel> analyticHasDefaults = analyticDefaultAndHasDefaultModel.AnalyticHasDefaultModels.Where(x => x.Score != null);

                int analyticDataId = (int)HttpContext.Session.GetInt32("AnalyticId");

                if (analyticDataId == -1)
                {
                    return RedirectToAction("Index");
                }

                foreach (var item in analyticHasDefaults)
                {
                    Service.AddScoreToAnalyticDefault(item.AnalyticDefaultId, analyticDataId, (int)item.Score);
                }
            }

            return RedirectToAction("Goal");
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
    }
}