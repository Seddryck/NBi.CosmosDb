﻿using NBi.Core.Query;
using NBi.Core.Query.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.Query.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents;

namespace NBi.Core.CosmosDb.Query.Command
{
    class GraphCommandFactory : BaseCommandFactory
    {
        public override bool CanHandle(IClient client) => client is GraphClient;

        protected override ICommand OnInstantiate(BaseClientOperation clientOperation, BaseCommandOperation commandOperation)
            => new GraphCommand((GraphClientOperation)clientOperation, (GraphCommandOperation)commandOperation);

        protected override BaseCommandOperation OnBuildCommandOperation(BaseClientOperation clientOperation, string statementText)
            => new GraphCommandOperation((GraphClientOperation)clientOperation, statementText);
    }
}
