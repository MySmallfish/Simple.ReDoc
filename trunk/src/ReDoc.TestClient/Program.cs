using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Simple.ComponentModel;
using Simple.Data;

namespace ReDoc.TestClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServiceProvider.Add<IDatabaseConnectionStringProvider, ReDocDatabaseConnectionProvider>();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PropertiesForm());
            //Application.Run(new Customers());
        }
    }
}
