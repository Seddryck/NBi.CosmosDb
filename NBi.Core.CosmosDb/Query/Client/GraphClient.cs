using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    class GraphClient : BaseClient
    {
        public const string GraphToken = "graph";

        protected string GraphId { get => CollectionId; }

        public override Type UnderlyingSessionType => typeof(GraphClientOperation);

        public override object CreateNew() => CreateClientOperation();

        public GraphClientOperation CreateClientOperation()
        {
            return new GraphClientOperation(new UriBuilder(Endpoint).Uri, AuthKey, DatabaseId, GraphId);
        }

        internal GraphClient(string subdomain, string api, string authKey, string databaseId, string graphId)
            : this("https", subdomain, api, 443, authKey, databaseId, graphId)
        { }

        internal GraphClient(string protocol, string subdomain, string api, int port, string authKey, string databaseId, string graphId)
            : this(protocol, subdomain, api, "azure.com", port, authKey, databaseId, graphId)
        { }

        internal GraphClient(string protocol, string subdomain, string api, string domain, int port, string authKey, string databaseId, string graphId)
            : this(new UriBuilder(protocol, $"{subdomain}.{api}.{domain}", port).Uri, authKey, databaseId, graphId)
        { }

        internal GraphClient(Uri endpoint, string authkey, string databaseId, string graphId)
            : base(endpoint, authkey, databaseId, graphId, "graph")
        { }
    }
}
