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

namespace ReDoc.TestClient
{
    public partial class Customers : Form
    {
        public Customers()
        {
            InitializeComponent();
        }

        private int GetTenantId()
        {
            return int.Parse(TenantId.Text);
        }

        private void GetCustomers_Click(object sender, EventArgs e)
        {
            ReloadCustomers();
        }

        private void ReloadCustomers()
        {
            var ds = new CustomersDataService();
            var query = new CustomerSearchQuery();
            CustomersGrid.DataSource = ds.GetCustomers(GetTenantId(), query);
        }


        private void AddCustomer_Click(object sender, EventArgs e)
        {
            var customers = CustomersGrid.DataSource as Customer[];
            var index = 1;
            if (customers != null)
            {
                index = customers.Length + 1;
            }
            var customer = new Customer
                                {
                                    Username ="יאיר",
                                    Name = "לקוח חדש #" + index,
                                    Address = "כתובת ללקוח " + index,
                                    City = "עיר ל" + index,
                                    Phone = "052525360",
                                    Email = "a@b.com",
                                    IdNumber = "IdNum#" + index,
                                    UniqueId = Guid.NewGuid()
                                };
            var ds = new CustomersDataService();
            ds.AddCustomer(GetTenantId(), customer);
            ReloadCustomers();
        }


        private void CustomersGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (CustomersGrid.SelectedRows.Count > 0)
            {
                var selected = (Guid)CustomersGrid.SelectedRows[0].Cells[3].Value;
                UniqueId.Text = selected.ToString("N");
            }
        }

        private void UpdateCustomer_Click(object sender, EventArgs e)
        {
            var ds = new CustomersDataService();
            var customer = ds.GetCustomerByUniqueId(GetTenantId(), new Guid(UniqueId.Text));
            if (customer != null)
            {
                customer.Name = customer.Name + "- ע";
                ds.UpdateCustomer(GetTenantId(), customer);
                ReloadCustomers();
            }
            
        }
    }
}
