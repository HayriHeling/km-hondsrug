using System;
using Eduria.Services;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Diagnostics;
using Microsoft.SqlServer;
using Microsoft.SqlServer.Management;
using Microsoft.AspNetCore.Mvc;

namespace Eduria.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseService DatabaseService;

        public AdminController(DatabaseService databaseService)
        {
            DatabaseService = databaseService;
        }
        public IActionResult Database()
        {
            BackupDatabase();
            return View();
        }

        public void BackupDatabase()
        {
            DatabaseService.Backup();
        }
    }
}