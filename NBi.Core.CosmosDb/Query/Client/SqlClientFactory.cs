using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    public class SqlClientFactory : BaseClientFactory
    {
        protected override bool CanHandle(DbConnectionStringBuilder connectionStringBuilder)
        {
            return (
                        connectionStringBuilder.ContainsKey(BaseClient.CollectionToken)
                        && HasApiSetTo(connectionStringBuilder, "sql")
                   );
        }

        
        public override IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var endpoint = new Uri((string)connectionStringBuilder[BaseClient.EndpointToken]);
            var authkey = (string)connectionStringBuilder[BaseClient.AuthKeyToken];
            var database = (string)connectionStringBuilder[BaseClient.DatabaseToken];
            var collection = (string)connectionStringBuilder[BaseClient.CollectionToken];

            return new SqlClient(endpoint, authkey, database, collection);
        }
    }
}
