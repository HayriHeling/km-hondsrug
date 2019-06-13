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
        private readonly DatabaseService _databaseService;

        public AdminController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        [HttpPost]
        public IActionResult Database()
        {
            return View();
        }

        public void BackupDatabase()
        {
            _databaseService.Backup("Download");
        }

        public void UploadDatabase()
        {
            _databaseService.Backup("Upload");
        }
    }
}