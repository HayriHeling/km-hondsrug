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
        private readonly AppSettingsService _appSettingsService;

        private const string DatabaseName = "Eduria_Development";

        private string _backupName;

        private const string BackupPlace = "D:\\";

        public DatabaseService(IOptions<AppSettingsService> appSettingsService)
        {
            _appSettingsService = appSettingsService.Value;
        }

        public void Backup(string param = "")
        {
            _backupName = BackupNameGenerator(param);
            SqlConnection sqlConnection = OpenConnection();
            string sqlQuery = QueryBuilder();
            SqlCommand sqlCommand = new SqlCommand(sqlQuery, sqlConnection) {CommandType = CommandType.Text};
            int iRows = sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public string QueryBuilder()
        {
            return "BACKUP DATABASE " + DatabaseName + "" +
                   " TO DISK = '" + BackupPlace + _backupName + "' " +
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
