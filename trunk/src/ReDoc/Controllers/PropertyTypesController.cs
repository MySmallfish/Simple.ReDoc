using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using ReDoc.Data;
using ReDoc.Models;
using Simple.ComponentModel;

namespace SimplyLog_Safety.Controllers
{
    public class PropertyTypesController : ApiController
    {
        public IEnumerable<PropertyType> Get()
        {
            var propertyTypesDataService = ServiceProvider.Get<IPropertyTypesDataService>();
            return propertyTypesDataService.GetPropertyTypes(1);
        }

    }
}
