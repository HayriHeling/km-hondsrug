using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.SqlServer.Management.Smo;

namespace Eduria.Services
{
    public class DatabaseService
    {
        private readonly AppSettingsService AppSettingsService;

        private readonly string DatabaseName = "Eduria_Development";

        private string BackupName;

        private readonly string BackupPlace = "D:\\";
        public DatabaseService(IOptions<AppSettingsService> appSettingsService)
        {
            AppSettingsService = appSettingsService.Value;
            BackupName = BackupNameGenerator();
        }

        public void Backup()
        {
            SqlConnection sqlConnection = OpenConnection();
            string sqlQuery = QueryBuilder();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection) {CommandType = CommandType.Text};
            int iRows = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public string QueryBuilder()
        {
            return "BACKUP DATABASE " + DatabaseName + "" +
                   " TO DISK = '" + BackupPlace + BackupName + "' " +
                   "WITH FORMAT, MEDIANAME = 'Z_SQLServerBackups', " +
                   "NAME = '" + BackupName + "';";
        }

        public SqlConnection OpenConnection()
        {
            SqlConnection sqlConnection = new SqlConnection {ConnectionString = AppSettingsService.EduriaDevelopment};
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
