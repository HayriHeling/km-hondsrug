using System.IO;
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

        public ActionResult Download()
        {
            string fullName = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\", "DatabaseBackup.bak");

            byte[] fileBytes = GetFile(fullName);
            return File(
                fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, "DatabaseBackup.bak");
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public void UploadDatabase()
        {
            _databaseService.Backup("Upload");
        }
    }
}