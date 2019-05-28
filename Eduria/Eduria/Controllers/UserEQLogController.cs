using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Services;
using EduriaData.Models;
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

        public void AddUserEqLog(UserEQLog userEqLog)
        {
            _userEQLogService.Add(userEqLog);
        }

        public void AddListOfUserEqLogs(List<UserEQLog> userEqLogs)
        {
            foreach (UserEQLog userEqLog in userEqLogs)
            {
                AddUserEqLog(userEqLog);
            }

            ;
        }
    }
}
