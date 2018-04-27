using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.CosmosDb.Query.Client
{
    public class GraphClientFactory : BaseClientFactory
    {
        protected override bool CanHandle(DbConnectionStringBuilder connectionStringBuilder)
        {
            return (
                    HasGraphToken(connectionStringBuilder)
                    || (
                        connectionStringBuilder.ContainsKey(GraphClient.CollectionToken)
                        && HasApiSetTo(connectionStringBuilder, "graph")
                       )
                   );
        }

        private bool HasGraphToken(DbConnectionStringBuilder connectionStringBuilder) => connectionStringBuilder.ContainsKey(GraphClient.GraphToken);
        public override IClient Instantiate(string connectionString)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            var endpoint = new Uri((string)connectionStringBuilder[BaseClient.EndpointToken]);
            var authkey = (string)connectionStringBuilder[BaseClient.AuthKeyToken];
            var database = (string)connectionStringBuilder[BaseClient.DatabaseToken];
            var graph = (string)
                (
                    connectionStringBuilder.ContainsKey(GraphClient.GraphToken)
                    ? connectionStringBuilder[GraphClient.GraphToken]
                    : connectionStringBuilder[BaseClient.CollectionToken]
                );

            return new GraphClient(endpoint, authkey, database, graph);
        }
    }
}
