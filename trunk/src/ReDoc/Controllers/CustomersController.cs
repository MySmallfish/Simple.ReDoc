using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ReDoc.Data;
using ReDoc.Models;
using Simple;
using Simple.ComponentModel;

namespace ReDoc.Controllers
{
    public class CustomersController : ApiController
    {
        public IEnumerable<Customer> Get(DateTime? lastModified, string userName)
        {
            var searchQuery = new CustomerSearchQuery()
                                  {
                                      LastModified = lastModified,
                                      Username = userName
                                  };

            var dataService = ServiceProvider.Get<ICustomersDataService>();
            return dataService.GetCustomers(1, searchQuery);
        }

        public void Post(Customer[] customers)
        {
            var dataService = ServiceProvider.Get<ICustomersDataService>();
            //customers.ForEach(customer => { dataService.SaveCustomer(1, customer); });
        }
    }
}
