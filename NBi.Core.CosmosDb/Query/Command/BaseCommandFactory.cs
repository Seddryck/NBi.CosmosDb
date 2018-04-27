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
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Documents;
using NBi.Extensibility;

namespace NBi.Core.CosmosDb.Query.Command
{
    abstract class BaseCommandFactory : ICommandFactory
    {
        public abstract bool CanHandle(IClient client);

        public ICommand Instantiate(IClient client, IQuery query)
            => Instantiate(client, query, null);

        public ICommand Instantiate(IClient client, IQuery query, ITemplateEngine engine)
        {
            if (!CanHandle(client))
                throw new ArgumentException();
            var clientOperation = (BaseClientOperation)client.CreateNew();
            var commandOperation = BuildCommandOperation(clientOperation, query, engine);
            return OnInstantiate(clientOperation, commandOperation);
        }

        protected abstract ICommand OnInstantiate(BaseClientOperation clientOperation, BaseCommandOperation commandOperation);

        protected BaseCommandOperation BuildCommandOperation(BaseClientOperation clientOperation, IQuery query, ITemplateEngine engine)
        {
            var statementText = query.Statement;

            if (query.TemplateTokens != null && query.TemplateTokens.Count() > 0 && engine!=null)
                statementText = ApplyVariablesToTemplate(engine, query.Statement, query.TemplateTokens);

            return OnBuildCommandOperation(clientOperation, statementText);
        }

        protected abstract BaseCommandOperation OnBuildCommandOperation(BaseClientOperation clientOperation, string statementText);

        private string ApplyVariablesToTemplate(ITemplateEngine engine, string template, IEnumerable<IQueryTemplateVariable> variables)
        {
            var valuePairs = new List<KeyValuePair<string, object>>();
            foreach (var variable in variables)
                valuePairs.Add(new KeyValuePair<string, object>(variable.Name, variable.Value));
            return engine.Render(template, valuePairs);
        }
    }
}
