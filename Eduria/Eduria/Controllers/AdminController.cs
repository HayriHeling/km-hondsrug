using System;
using Eduria.Models;
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
        
        public IActionResult Database()
        {
            return View();
        }

        /// <summary>
        /// This method calls the backup method in the databaseservice
        /// </summary>
        public IActionResult BackupDatabase()
        {
            string backupData = _databaseService.Backup();
            return View(new DatabaseModel
            {
                BackupData = backupData,
                ConnectionString = "",
                Name = "EduriaData"
            });
        }

        public void UploadDatabase()
        {
            _databaseService.Backup("Upload");
        }
    }
}