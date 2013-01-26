using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Web;
using ReDoc.Models;
using Simple.Data;
using Simple.ReDoc.Data;

namespace ReDoc.Data
{
    public class CustomersDataService : ReDocDataService, ICustomersDataService
    {

        public Customer GetCustomerByUniqueId(int tenantId, Guid uniqueId)
        {
            var result = default(Customer);
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
                                   result = CreateCustomerFromData(reader);
                               }
                           });
            return result;
        }
        public Customer[] GetCustomers(int tenantId, CustomerSearchQuery searchQuery)
        {
            var customers = new List<Customer>();
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
                                    var customer = CreateCustomerFromData(reader);
                                    customers.Add(customer);
                                }
                            });

            return customers.ToArray();
        }

        private string GetReadSql()
        {
            var sql = "SELECT * FROM Customer WHERE TenantId = @TenantId";
            return sql;
        }

        private Customer CreateCustomerFromData(DbDataReader reader)
        {
            var customer = new Customer();
            customer.Id = (int) reader["Id"];
            customer.Username = DataHelper.GetStringValueOrEmptyString(reader["Username"]);
            customer.Name = DataHelper.GetStringValueOrEmptyString(reader["Name"]);
            customer.IdNumber = DataHelper.GetStringValueOrEmptyString(reader["IdNumber"]);
            customer.UniqueId = (Guid) reader["UniqueId"];
            customer.Phone = DataHelper.GetStringValueOrEmptyString(reader["Phone"]);
            customer.Email = DataHelper.GetStringValueOrEmptyString(reader["Email"]);
            customer.Address = DataHelper.GetStringValueOrEmptyString(reader["Address"]);
            customer.City = DataHelper.GetStringValueOrEmptyString(reader["City"]);
            customer.LastModified = DataHelper.DateAsUtc(DataHelper.GetSafeValue<DateTime?>(reader["LastModified"]));
            customer.CreatedAt = DataHelper.DateAsUtc(DataHelper.GetSafeValue<DateTime>(reader["CreatedAt"]));
            return customer;
        }


        public void AddCustomer(int tenantId, Customer customer)
        {
            var sql = BuildInsertCustomerSql();
            ExecuteNonQuery(
                sql, CommandType.Text,
                command =>
                    {
                        AddTenantParameters(command, tenantId);
                        AddCommonParameters(customer, command);
                    });
        }
        public void UpdateCustomer(int tenantId, Customer customer)
        {
            var sql = BuildUpdateCustomerSql();
            ExecuteNonQuery(
                sql, CommandType.Text,
                command =>
                    {
                        AddTenantParameters(command, tenantId);
                        AddCommonParameters(customer, command);
                        DataHelper.AddParameterWithValue(command, "@Id", DbType.Int32, customer.Id);
                    });
        }

        private static void AddCommonParameters(Customer customer, DbCommand command)
        {
            DataHelper.AddParameterWithValue(command, "@UserName", DbType.String, customer.Username);
            DataHelper.AddParameterWithValue(command, "@Name", DbType.String, customer.Name);
            DataHelper.AddParameterWithValue(command, "@IdNumber", DbType.String, customer.IdNumber);
            DataHelper.AddParameterWithValue(command, "@Phone", DbType.String, customer.Phone);
            DataHelper.AddParameterWithValue(command, "@Email", DbType.String, customer.Email);
            DataHelper.AddParameterWithValue(command, "@Address", DbType.String, customer.Address);
            DataHelper.AddParameterWithValue(command, "@City", DbType.String, customer.City);
            DataHelper.AddParameterWithValue(command, "@UniqueId", DbType.Guid, customer.UniqueId);
            DataHelper.AddParameterWithValue(command, "@CreatedAt", DbType.DateTime, DateTime.UtcNow);
        }

        private string BuildInsertCustomerSql()
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
        private string BuildUpdateCustomerSql()
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


        public int? GetCustomerIdByUniqueId(int tenantId, Guid uniqueId)
        {
            var sql = "SELECT Id FROM Customer WHERE TenantId = @TenantId AND UniqueId = @UniqueId";
            return ExecuteScalar(sql, CommandType.Text, default(int?), command =>
                                                                           {
                                                                               AddTenantParameters(command, tenantId);
                                                                               DataHelper.AddParameterWithValue(
                                                                                   command, "@UniqueId", DbType.Guid,
                                                                                   uniqueId);
                                                                           });
        }


    }
}