using Eduria.Models;
using Eduria.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Eduria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly DatabaseService _databaseService;

        public AdminController(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IActionResult Index()
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