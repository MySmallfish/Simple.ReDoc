using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReDoc.Data;
using Simple.ComponentModel;
using Simple.Data;
using Simple.ReDoc.Data;

namespace ReDoc.App_Start
{
    public class ServicesConfig
    {
        public static void Register()
        {
            ServiceProvider.Add<IDatabaseConnectionStringProvider, ReDocDatabaseConnectionProvider>();
            ServiceProvider.Add<IPropertyTypesDataService, PropertyTypesDataService>();
            ServiceProvider.Add<ICustomersDataService, CustomersDataService>();
        }
    }
}