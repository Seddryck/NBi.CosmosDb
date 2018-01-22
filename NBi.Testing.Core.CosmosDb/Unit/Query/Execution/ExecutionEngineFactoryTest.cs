using Moq;
using NBi.Core.Configuration;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Core.CosmosDb.Query.Execution;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Unit.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        private class GraphConfig : IExtensionsConfiguration
        {
            public IReadOnlyCollection<Type> Extensions => new List<Type>()
            {
                typeof(GraphClientFactory),
                typeof(GraphCommandFactory),
                typeof(GraphExecutionEngine),
            };
        }

        [Test]
        public void Instantiate_GraphConnectionString_GraphExecutionEngine()
        {
            var config = new GraphConfig();
            var clientProvider = new ClientProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(clientProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == $"Endpoint=https://xyz.documents.azure.com:443;AuthKey={base64AuthKey};database=db;graph=FoF"
                    && x.Statement == "g.V().Count()"
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<GraphExecutionEngine>());
        }

        private class SqlConfig : IExtensionsConfiguration
        {
            public IReadOnlyCollection<Type> Extensions => new List<Type>()
            {
                typeof(SqlClientFactory),
                typeof(SqlCommandFactory),
                typeof(SqlExecutionEngine),
            };
        }

        [Test]
        public void Instantiate_SqlConnectionString_SqlExecutionEngine()
        {
            var config = new SqlConfig();
            var clientProvider = new ClientProvider(config);
            var commandProvider = new CommandProvider(config);
            var factory = new ExecutionEngineFactory(clientProvider, commandProvider, config);

            var query = Mock.Of<IQuery>
                (
                    x => x.ConnectionString == $"Endpoint=https://xyz.documents.azure.com:443;AuthKey={base64AuthKey};database=db;collection=FoF;api=sql"
                    && x.Statement == "SELECT * FROM Families f WHERE f.id = \"WakefieldFamily\""
                );

            var engine = factory.Instantiate(query);
            Assert.That(engine, Is.Not.Null);
            Assert.That(engine, Is.TypeOf<SqlExecutionEngine>());
        }
    }
}
