using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Extensibility.Query;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

namespace NBi.Core.CosmosDb.Query.Command
{
    class SqlCommandFactory : BaseCommandFactory
    {
        public override bool CanHandle(IClient client) => client is SqlClient;

        protected override ICommand OnInstantiate(BaseClientOperation clientOperation, BaseCommandOperation commandOperation)
            => new SqlCommand((SqlClientOperation)clientOperation, (SqlCommandOperation)commandOperation);

        protected override BaseCommandOperation OnBuildCommandOperation(BaseClientOperation clientOperation, string statementText)
            => new SqlCommandOperation((SqlClientOperation)clientOperation, statementText);
    }
}
