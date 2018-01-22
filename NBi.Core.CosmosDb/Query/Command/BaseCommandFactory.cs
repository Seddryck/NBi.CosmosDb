using NBi.Core.Query;
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
    abstract class BaseCommandFactory : ICommandFactory
    {
        public abstract bool CanHandle(IClient client);

        public ICommand Instantiate(IClient client, IQuery query)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var clientOperation = (BaseClientOperation)client.CreateNew();
            var commandOperation = BuildCommandOperation(clientOperation, query);
            return OnInstantiate(clientOperation, commandOperation);
        }

        protected abstract ICommand OnInstantiate(BaseClientOperation clientOperation, BaseCommandOperation commandOperation);

        protected BaseCommandOperation BuildCommandOperation(BaseClientOperation clientOperation, IQuery query)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0)
                statementText = ApplyVariablesToTemplate(query.Statement, query.TemplateTokens);

            return OnBuildCommandOperation(clientOperation, statementText);
        }

        protected abstract BaseCommandOperation OnBuildCommandOperation(BaseClientOperation clientOperation, string statementText);

        private string ApplyVariablesToTemplate(string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var templateEngine = new StringTemplateEngine(template, variables);
            return templateEngine.Build();
        }
    }
}
