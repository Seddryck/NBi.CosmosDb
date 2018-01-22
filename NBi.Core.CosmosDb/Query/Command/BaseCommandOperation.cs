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
    class BaseCommandOperation
    {
        public string PreparedStatement { get; }
        public BaseClientOperation Client { get; }
        public BaseCommandOperation(BaseClientOperation client, string preparedStatement)
        {
            Client = client;
            PreparedStatement = preparedStatement;
        }

        public IDocumentQuery<dynamic> Create() => Client.CreateCommand(PreparedStatement);
    }
}
