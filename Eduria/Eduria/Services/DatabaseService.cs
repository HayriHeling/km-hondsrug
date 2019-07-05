using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Extensions.Options;

namespace Eduria.Services
{
    public class DatabaseService
    {
        private readonly AppSettingsService _appSettingsService;

        private const string DatabaseName = "Eduria_Development";

        private string _backupName;

        private string BackupPlace;

        public DatabaseService(IOptions<AppSettingsService> appSettingsService)
        {
            _appSettingsService = appSettingsService.Value;
            BackupPlace = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Content\\", "DatabaseBackup.bak");
        }

        /// <summary>
        /// This method creates a backup of the database server.
        /// </summary>
        /// <param name="param"></param>
        public string Backup(string param = "")
        {
            _backupName = BackupNameGenerator(param);
            SqlConnection sqlConnection = OpenConnection();
            string sqlQuery = QueryBuilder();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection) {CommandType = CommandType.Text};
            int iRows = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
            return BackupPlace;
        }

        public string QueryBuilder()
        {
            return "BACKUP DATABASE " + DatabaseName + "" +
                   " TO DISK = '" + BackupPlace + "' " +
                   "WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', " +
                   "NAME = '" + _backupName + "';";
        }

        public SqlConnection OpenConnection()
        {
            SqlConnection sqlConnection = new SqlConnection {ConnectionString = _appSettingsService.EduriaDevelopment};
            sqlConnection.Open();
            return sqlConnection;
        }

        public string BackupNameGenerator(string additionalParam = "")
        {
            string dateTimeSaving = DateTime.Now.ToString().Replace(":", "").Replace("-", "").Replace(" ", "");
            return "" + dateTimeSaving + "" + additionalParam + ".bak";
        }
    }
}
