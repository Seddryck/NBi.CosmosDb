using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Moq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.CosmosDb.Integration.Query.Command
{
    [TestFixture]
    public class GraphCommandFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void Instantiate_NoParameter_CorrectResultSet()
        {
            GraphClient client = new GraphClientFactory().Instantiate(ConnectionStringReader.GetLocaleGraph()) as GraphClient;
            var query = Mock.Of<IQuery>(
                x => x.Statement == "g.V()"
                );
            var factory = new GraphCommandFactory();
            var cosmosdbQuery = (factory.Instantiate(client, query, null).Implementation) as GraphCommandOperation;
            var statement = cosmosdbQuery.Create();

            var session = client.CreateClientOperation();
            var results = session.Run(statement);
            Assert.That(results.Count, Is.EqualTo(4));
        }
    }
}
