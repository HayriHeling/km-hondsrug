using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Services;
using EduriaData.Models;
using EduriaData.Models.ExamLayer;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class UserEQLogController
    {
        private UserEQLogService _userEQLogService;

        public UserEQLogController(UserEQLogService userEqLogService)
        {
            _userEQLogService = userEqLogService;
        }

        /// <summary>
        /// Method to add a list of userEqLogs to the database.
        /// </summary>
        /// <param name="userEqLogs">The list of userEqLogs to be added to the database.</param>
        public void AddListOfUserEqLogs(List<UserEQLog> userEqLogs)
        {
            foreach (UserEQLog userEqLog in userEqLogs)
            {
                _userEQLogService.Add(userEqLog);
            }
        }
    }
}