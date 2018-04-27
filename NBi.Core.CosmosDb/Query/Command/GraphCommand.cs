using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GraphCommand : BaseCommand
    {
        public GraphCommand(GraphClientOperation client, GraphCommandOperation command)
            : base(client, command)
        { }
    }
}
