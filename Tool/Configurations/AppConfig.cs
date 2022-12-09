using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tool.Utilities;

namespace Tool.Configurations
{
    public static class AppConfig
    {
        public static ClientConfig Client
        {
            get
            {
                var client = new ClientConfig();
                Configuration.GetSection("client").Bind(client);
                return client;
            }
        }

        public static CookieConfig Cookie
        {
            get
            {
                var cookie = new CookieConfig();
                Configuration.GetSection("cookie").Bind(cookie);
                return cookie;
            }
        }
        public static SessionConfig Session
        {
            get
            {
                var session = new SessionConfig();
                Configuration.GetSection("session").Bind(session);
                return session;
            }
        }

        public static ConnectionConfig Connection
        {
            get
            {
                var connection = new ConnectionConfig();
                Configuration.GetSection("connection").Bind(connection);
                return connection;
            }
        }

        private static IConfigurationRoot Configuration
        {
            get
            {
                return Util.GetConfiguration("Application", "appconfig.json");
            }
        }
    }
}
