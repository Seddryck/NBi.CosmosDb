using NBi.Core.CosmosDb.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Command
{
    class SqlCommandOperation : BaseCommandOperation
    {
        public SqlCommandOperation(SqlClientOperation client, string preparedStatement)
            : base(client, preparedStatement)
        { }
    }
}
