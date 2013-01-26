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
            property.CreatedAt = DataHelper.DateAsUtc(DataHelper.GetSafeValue<DateTime>(reader["CreatedAt"]));
            property.LastModified = DataHelper.DateAsUtc(DataHelper.GetSafeValue<DateTime?>(reader["LastModified"]));
            property.Floor = DataHelper.GetStringValueOrEmptyString(reader["Floor"]);
            property.Rooms = DataHelper.GetSafeValue(reader["Rooms"], 0);
            property.PercentsRate = DataHelper.GetSafeValue(reader["PercentsRate"], default(double?));
            property.AmountRate = DataHelper.GetSafeValue(reader["AmountRate"], default(double?));
            property.RequestedPrice = DataHelper.GetSafeValue(reader["RequestedPrice"], default(double?));
            property.Comments = DataHelper.GetStringValueOrEmptyString(reader["Comments"]);
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
            DataHelper.AddParameterWithValue(command, "@SellerId", DbType.Int32, property.SellerId);
            DataHelper.AddParameterWithValue(command, "@Type", DbType.Int32, property.PropertyTypeId);
            DataHelper.AddParameterWithValue(command, "@UserName", DbType.String, property.Username);
            DataHelper.AddParameterWithValue(command, "@RequestedPrice", DbType.Double, DataHelper.ValueOrDBNull(property.RequestedPrice));
            DataHelper.AddParameterWithValue(command, "@AmountRate", DbType.Double, DataHelper.ValueOrDBNull(property.AmountRate));
            DataHelper.AddParameterWithValue(command, "@PercentsRate", DbType.Double, DataHelper.ValueOrDBNull(property.PercentsRate));
            DataHelper.AddParameterWithValue(command, "@Rooms", DbType.Int32, property.Rooms);
            DataHelper.AddParameterWithValue(command, "@Comments", DbType.String, DataHelper.ValueOrDBNull(property.Comments));
            DataHelper.AddParameterWithValue(command, "@Floor", DbType.String, DataHelper.ValueOrDBNull(property.Floor));
            DataHelper.AddParameterWithValue(command, "@Address", DbType.String, DataHelper.ValueOrDBNull(property.Address));
            DataHelper.AddParameterWithValue(command, "@City", DbType.String, DataHelper.ValueOrDBNull(property.City));
            DataHelper.AddParameterWithValue(command, "@UniqueId", DbType.Guid, DataHelper.ValueOrDBNull(property.UniqueId));
            DataHelper.AddParameterWithValue(command, "@CreatedAt", DbType.DateTime, DateTime.UtcNow);
        }

        private string BuildInsertPropertySql()
        {
            var sql = new StringBuilder("INSERT INTO ");
            sql.Append("[Property] (");
            sql.Append("TenantId");
            sql.Append(", ");
            sql.Append("Username");
            sql.Append(", ");
            sql.Append("SellerId");
            sql.Append(", ");
            sql.Append("Type");
            sql.Append(", ");
            sql.Append("Rooms");
            sql.Append(", ");
            sql.Append("Floor");
            sql.Append(", ");
            sql.Append("Address");
            sql.Append(", ");
            sql.Append("City");
            sql.Append(", ");
            sql.Append("UniqueId");
            sql.Append(", ");
            sql.Append("CreatedAt");
            sql.Append(", ");
            sql.Append("PercentsRate");
            sql.Append(", ");
            sql.Append("AmountRate");
            sql.Append(", ");
            sql.Append("RequestedPrice");
            sql.Append(", ");
            sql.Append("Comments");
            sql.Append(") Values(");
            sql.Append("@TenantId");
            sql.Append(", ");
            sql.Append("@Username");
            sql.Append(", ");
            sql.Append("@SellerId");
            sql.Append(", ");
            sql.Append("@Type");
            sql.Append(", ");
            sql.Append("@Rooms");
            sql.Append(", ");
            sql.Append("@Floor");
            sql.Append(", ");
            sql.Append("@Address");
            sql.Append(", ");
            sql.Append("@City");
            sql.Append(", ");
            sql.Append("@UniqueId");
            sql.Append(", ");
            sql.Append("@CreatedAt");
            sql.Append(", ");
            sql.Append("@PercentsRate");
            sql.Append(", ");
            sql.Append("@AmountRate");
            sql.Append(", ");
            sql.Append("@RequestedPrice");
            sql.Append(", ");
            sql.Append("@Comments");
            sql.Append(")");

            return sql.ToString();
        }
        private string BuildUpdatePropertySql()
        {
            var sql = new StringBuilder("UPDATE ");
            sql.Append("[Property] SET ");
            sql.Append("Username");
            sql.Append(" = ");
            sql.Append("@Username");
            sql.Append(", ");
            sql.Append("SellerId");
            sql.Append(" = ");
            sql.Append("@SellerId");
            sql.Append(", ");
            sql.Append("Type");
            sql.Append(" = ");
            sql.Append("@Type");
            sql.Append(", ");
            sql.Append("Rooms");
            sql.Append(" = ");
            sql.Append("@Rooms");
            sql.Append(", ");
            sql.Append("Floor");
            sql.Append(" = ");
            sql.Append("@Floor");
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
            sql.Append(", ");
            sql.Append("PercentsRate");
            sql.Append(" = ");
            sql.Append("@PercentsRate");
            sql.Append(", ");
            sql.Append("AmountRate");
            sql.Append(" = ");
            sql.Append("@AmountRate");
            sql.Append(", ");
            sql.Append("RequestedPrice");
            sql.Append(" = ");
            sql.Append("@RequestedPrice");
            sql.Append(", ");
            sql.Append("Comments");
            sql.Append(" = ");
            sql.Append("@Comments");
            sql.Append(" WHERE TenantId = @TenantId AND Id = @Id");
            return sql.ToString();
        }
    }
}
