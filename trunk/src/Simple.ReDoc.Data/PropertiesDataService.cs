using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReDoc.Models;
using Simple.Data;
using Simple.ReDoc.Data;

namespace Simple.ReDoc.Contracts
{
    public class PropertiesDataService : ReDocDataService, IPropertyDataService
    {
        public Property GetPropertyByUniqueId(int tenantId, Guid uniqueId)
        {
            var result = default(Property);
            var sql = GetReadSql();
            sql += " AND UniqueId = @UniqueId";
            ExecuteRreader(sql, CommandType.Text,
                            command =>
                            {
                                AddTenantParameters(command, tenantId);
                                DataHelper.AddParameterWithValue(command, "@UniqueId", DbType.Guid, uniqueId);
                            },
                           reader =>
                           {
                               if (reader.Read())
                               {
                                   result = CreatePropertyFromData(reader);
                               }
                           });
            return result;
        }
        
        public Property[] GetProperties(int tenantId, DateTime? lastModified)
        {
            var properties = new List<Property>();
            var sql = GetReadSql();

            ExecuteRreader(sql, CommandType.Text,
                            command =>
                            {
                                AddTenantParameters(command, tenantId);

                            },
                           reader =>
                           {
                               while (reader.Read())
                               {
                                   var property = CreatePropertyFromData(reader);
                                   properties.Add(property);
                               }
                           });

            return properties.ToArray();
        }

        private string GetReadSql()
        {
            var sql = "SELECT * FROM Property WHERE TenantId = @TenantId";
            return sql;
        }

        private Property CreatePropertyFromData(DbDataReader reader)
        {
            var property = new Property();
            property.Id = (int)reader["Id"];
            property.Username = DataHelper.GetStringValueOrEmptyString(reader["Username"]);
            property.UniqueId = (Guid)reader["UniqueId"];
            property.Address = DataHelper.GetStringValueOrEmptyString(reader["Address"]);
            property.City = DataHelper.GetStringValueOrEmptyString(reader["City"]);
            property.LastModified = DataHelper.DateAsUtc(DataHelper.GetSafeValue<DateTime?>(reader["LastModified"]));
            // ...
            return property;
        }

        public void AddProperty(int tenantId, Property property)
        {
            var sql = BuildInsertPropertySql();
            ExecuteNonQuery(
                sql, CommandType.Text,
                command =>
                {
                    AddTenantParameters(command, tenantId);
                    AddCommonParameters(property, command);
                });
        }

        public void UpdateProperty(int tenantId, Property property)
        {
            var sql = BuildUpdatePropertySql();
            ExecuteNonQuery(
                sql, CommandType.Text,
                command =>
                {
                    AddTenantParameters(command, tenantId);
                    AddCommonParameters(property, command);
                    DataHelper.AddParameterWithValue(command, "@Id", DbType.Int32, property.Id);
                });
        }


        private static void AddCommonParameters(Property property, DbCommand command)
        {
            DataHelper.AddParameterWithValue(command, "@UserName", DbType.String, property.Username);
            // ...
            DataHelper.AddParameterWithValue(command, "@Address", DbType.String, property.Address);
            DataHelper.AddParameterWithValue(command, "@City", DbType.String, property.City);
            DataHelper.AddParameterWithValue(command, "@UniqueId", DbType.Guid, property.UniqueId);
            DataHelper.AddParameterWithValue(command, "@CreatedAt", DbType.DateTime, DateTime.UtcNow);
        }

        private string BuildInsertPropertySql()
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.Append("[Customer] (");
            sql.Append("TenantId");
            sql.Append(", ");
            sql.Append("Username");
            sql.Append(", ");
            sql.Append("Name");
            sql.Append(", ");
            sql.Append("IdNumber");
            sql.Append(", ");
            sql.Append("Phone");
            sql.Append(", ");
            sql.Append("Email");
            sql.Append(", ");
            sql.Append("Address");
            sql.Append(", ");
            sql.Append("City");
            sql.Append(", ");
            sql.Append("UniqueId");
            sql.Append(", ");
            sql.Append("CreatedAt");
            sql.Append(") Values(");
            sql.Append("@TenantId");
            sql.Append(", ");
            sql.Append("@Username");
            sql.Append(", ");
            sql.Append("@Name");
            sql.Append(", ");
            sql.Append("@IdNumber");
            sql.Append(", ");
            sql.Append("@Phone");
            sql.Append(", ");
            sql.Append("@Email");
            sql.Append(", ");
            sql.Append("@Address");
            sql.Append(", ");
            sql.Append("@City");
            sql.Append(", ");
            sql.Append("@UniqueId");
            sql.Append(", ");
            sql.Append("@CreatedAt");
            sql.Append(")");

            return sql.ToString();
        }
        private string BuildUpdatePropertySql()
        {
            var sql = new StringBuilder("UPDATE ");
            sql.Append("[Customer] SET ");
            sql.Append("Username");
            sql.Append(" = ");
            sql.Append("@Username");
            sql.Append(", ");
            sql.Append("Name");
            sql.Append(" = ");
            sql.Append("@Name");
            sql.Append(", ");
            sql.Append("IdNumber");
            sql.Append(" = ");
            sql.Append("@IdNumber");
            sql.Append(", ");
            sql.Append("Phone");
            sql.Append(" = ");
            sql.Append("@Phone");
            sql.Append(", ");
            sql.Append("Email");
            sql.Append(" = ");
            sql.Append("@Email");
            sql.Append(", ");
            sql.Append("Address");
            sql.Append(" = ");
            sql.Append("@Address");
            sql.Append(", ");
            sql.Append("City");
            sql.Append(" = ");
            sql.Append("@City");
            sql.Append(", ");
            sql.Append("UniqueId");
            sql.Append(" = ");
            sql.Append("@UniqueId");
            sql.Append(", ");
            sql.Append("CreatedAt");
            sql.Append(" = ");
            sql.Append("@CreatedAt");
            sql.Append(" WHERE TenantId = @TenantId AND Id = @Id");
            return sql.ToString();
        }
    }
}
