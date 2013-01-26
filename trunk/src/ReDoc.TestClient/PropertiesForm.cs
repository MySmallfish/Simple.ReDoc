using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ReDoc.Data;
using ReDoc.Models;
using Simple.ReDoc.Contracts;

namespace ReDoc.TestClient
{
    public partial class PropertiesForm : Form
    {
        public PropertiesForm()
        {
            InitializeComponent();
        }

        private int GetTenantId()
        {
            return int.Parse(TenantId.Text);
        }

        private void GetCustomers_Click(object sender, EventArgs e)
        {
            ReloadProperties();
        }

        private void ReloadProperties()
        {
            var ds = new PropertiesDataService();
            PropertiesGrid.DataSource = ds.GetProperties(GetTenantId(), null);
        }


        private void AddCustomer_Click(object sender, EventArgs e)
        {
            var customers = PropertiesGrid.DataSource as Customer[];
            var index = 1;
            if (customers != null)
            {
                index = customers.Length + 1;
            }
            var property = new Property
                                {
                                    Username ="יאיר",
                                    Address = "כתובת ללקוח " + index,
                                    City = "עיר ל" + index,
                                    UniqueId = Guid.NewGuid(),
                                    //...
                                };
            var ds = new PropertiesDataService();
            ds.AddProperty(GetTenantId(), property);
            ReloadProperties();
        }


        private void CustomersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (PropertiesGrid.SelectedRows.Count > 0)
            {
                var selected = (Guid)PropertiesGrid.SelectedRows[0].Cells[1].Value;
                UniqueId.Text = selected.ToString("N");
            }
        }

        private void UpdateCustomer_Click(object sender, EventArgs e)
        {
            var ds = new PropertiesDataService();
            var customer = ds.GetPropertyByUniqueId(GetTenantId(), new Guid(UniqueId.Text));
            if (customer != null)
            {
                customer.Address = customer.Address + "- ע";
                ds.UpdateProperty(GetTenantId(), customer);
                ReloadProperties();
            }
            
        }
    }
}
