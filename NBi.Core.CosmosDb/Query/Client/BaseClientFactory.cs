using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.CosmosDb.Query.Client
{
    public abstract class BaseClientFactory : IClientFactory
    {
        public bool CanHandle(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            return connectionStringBuilder.ContainsKey(GraphClient.EndpointToken)
                && connectionStringBuilder.ContainsKey(GraphClient.AuthKeyToken)
                && connectionStringBuilder.ContainsKey(GraphClient.DatabaseToken)
                && CanHandle(connectionStringBuilder);
        }

        protected bool HasApiSetTo(DbConnectionStringBuilder connectionStringBuilder, string api)
            => connectionStringBuilder.ContainsKey(BaseClient.ApiToken) && ((string)connectionStringBuilder[BaseClient.ApiToken]).ToLowerInvariant() == api.ToLowerInvariant();


        protected abstract bool CanHandle(DbConnectionStringBuilder connectionStringBuilder);

        public abstract IClient Instantiate(string connectionString);
    }
}
