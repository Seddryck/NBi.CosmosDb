using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    class SqlClient : BaseClient
    {
        public override Type UnderlyingSessionType => typeof(SqlClientOperation);

        public override object CreateNew() => CreateClientOperation();

        public SqlClientOperation CreateClientOperation()
        {
            return new SqlClientOperation(new UriBuilder(Endpoint).Uri, AuthKey, DatabaseId, CollectionId);
        }

        public SqlClient(Uri endpoint, string authKey, string databaseid, string collectionId)
            : base(endpoint, authKey, databaseid, collectionId, "sql")
        { }
    }
}
