using System;
using System.Collections.Generic;
using System.Text;

namespace Tool.Configurations
{
    public class ConnectionConfig
    {
        public int Set { get; set; }
        public string Server { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string Scheme { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string[] Odbc { get; set; }
    }
}
