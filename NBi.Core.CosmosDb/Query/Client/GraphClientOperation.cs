using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    class GraphClientOperation : BaseClientOperation
    {
        public GraphClientOperation(Uri endpoint, string authKey, string databaseId, string graphId)
            : base(endpoint, authKey, databaseId, graphId)
        { }

        protected override IDocumentQuery<dynamic> OnCreateCommand(string preparedStatement)
            => Client.CreateGremlinQuery<dynamic>(Collection, preparedStatement);
    }
}
    