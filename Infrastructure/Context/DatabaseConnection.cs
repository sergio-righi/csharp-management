using Microsoft.Extensions.Configuration;
using System.IO;
using Tool.Configurations;
using Tool.Utilities;

namespace Infrastructure.Context
{
    public class DatabaseConnection
    {
        public static IConfiguration ConnectionConfiguration
        {
            get
            {
                return Util.GetConfiguration("Infrastructure");
            }
        }

        public static string ConnectionString
        {
            get
            {
                return string.Format(AppConfig.Connection.Odbc[AppConfig.Connection.Set], AppConfig.Connection.Server, AppConfig.Connection.Port, AppConfig.Connection.Database, AppConfig.Connection.Username, AppConfig.Connection.Password);
            }
        }
    }
}
