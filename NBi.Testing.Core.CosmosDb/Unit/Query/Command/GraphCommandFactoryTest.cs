using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBiClient = NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.CosmosDb.Query.Command;
using Microsoft.Azure.Documents.Linq;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.CosmosDb.Query.Command
{
    public class GraphCommandFactoryTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void CanHandle_GraphClient_True()
        {
            var client = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            Assert.That(factory.CanHandle(client), Is.True);
        }

        [Test]
        public void CanHandle_OtherKindOfClient_False()
        {
            var client = Mock.Of<IClient>();
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            Assert.That(factory.CanHandle(client), Is.False);
        }

        [Test]
        public void Instantiate_GraphClientAndQuery_CommandNotNull()
        {
            var client = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(client, query);
            Assert.That(command, Is.Not.Null);
        }

        [Test]
        public void Instantiate_GraphClientAndQuery_CommandImplementationCorrectType()
        {
            var client = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Implementation;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.TypeOf<GraphCommandOperation>());
        }

        [Test]
        public void Instantiate_GraphClientAndQuery_ClientCorrectType()
        {
            var client = new GraphClient("xyz", "graphs", base64AuthKey, "db", "FoF");
            var query = Mock.Of<IQuery>();
            var factory = new GraphCommandFactory();
            var command = factory.Instantiate(client, query);
            var impl = command.Client;
            Assert.That(impl, Is.Not.Null);
            Assert.That(impl, Is.InstanceOf<GraphClientOperation>());
        }
    }
}
