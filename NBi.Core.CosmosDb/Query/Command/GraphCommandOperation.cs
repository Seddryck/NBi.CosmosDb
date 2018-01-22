using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using Microsoft.Azure.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Graphs.Elements;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GraphCommandOperation : BaseCommandOperation
    {
        public GraphCommandOperation(GraphClientOperation client, string preparedStatement)
            : base(client, preparedStatement)
        { }
    }
}
