using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eduria.Services;
using EduriaData.Models;

namespace Eduria.Controllers
{
    public class UserEQLogController
    {
        private UserEQLogService _userEQLogService;

        public UserEQLogController(UserEQLogService userEqLogService)
        {
            _userEQLogService = userEqLogService;
        }

        public void AddUserEQLog(UserEQLog userEqLog)
        {
            _userEQLogService.Add(userEqLog);
        }
    }
}
