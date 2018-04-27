using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.CosmosDb.Query.Command
{
    abstract class BaseCommand : ICommand
    {
        public object Implementation { get; }
        public object Client { get; }

        public BaseCommand(BaseClientOperation client, BaseCommandOperation query)
        {
            Client = client;
            Implementation = query;
        }
    }
}
