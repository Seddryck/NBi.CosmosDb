using Microsoft.Azure.Documents.Linq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Command
{
    class SqlCommand : BaseCommand
    {
        public SqlCommand(SqlClientOperation client, SqlCommandOperation command)
            : base(client, command)
        { }
    }
}
