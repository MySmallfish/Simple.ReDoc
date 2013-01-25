using System.Collections.Generic;
using System.Data;
using ReDoc.Data;
using ReDoc.Models;
using Simple.Data;

namespace Simple.ReDoc.Data
{
    public class PropertyTypesDataService : ReDocDataService, IPropertyTypesDataService
    {
        public PropertyType[] GetPropertyTypes(int tenantId)
        {
            var propertyTypes = new List<PropertyType>();
            var sql = "SELECT Id, Name FROM PropertyType WHERE TenantId = @TenantId";

            ExecuteRreader(sql, CommandType.Text, 
                            command =>
                            {
                                AddTenantParameters(command, tenantId);
                            },
                            reader =>
                            {
                                while (reader.Read())
                                {
                                    var propertyType = new PropertyType();
                                    propertyType.Id = (int) reader["Id"];
                                    propertyType.Name = DataHelper.GetStringValueOrEmptyString(reader["Name"]);
                                    propertyTypes.Add(propertyType);
                                }       
                            });

            return propertyTypes.ToArray();
        }
    }
}