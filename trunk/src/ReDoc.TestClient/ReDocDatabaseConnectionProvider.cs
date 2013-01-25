using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Simple.Data;

namespace ReDoc
{
    public class ReDocDatabaseConnectionProvider : IDatabaseConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }
    }
}