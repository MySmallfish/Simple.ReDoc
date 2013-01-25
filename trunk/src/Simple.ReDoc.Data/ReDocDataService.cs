using System.Data;
using System.Data.Common;
using Simple.Data;

namespace Simple.ReDoc.Data
{
    public class ReDocDataService : DataServiceBase
    {
        
        protected virtual void AddTenantParameters(DbCommand command, int tenantId)
        {
            DataHelper.AddParameterWithValue(command, "@TenantId", DbType.Int32, tenantId);
        }
    }
}