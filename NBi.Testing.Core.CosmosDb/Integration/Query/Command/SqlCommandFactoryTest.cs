using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using Moq;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.CosmosDb.Query.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NBi.Testing.Core.CosmosDb.Integration.Query.Command
{
    [TestFixture]
    public class SqlCommandFactoryTest
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
            var client = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var query = Mock.Of<IQuery>(
                x => x.Statement == "SELECT * FROM FoF f WHERE f.label='person'"
                );
            var factory = new SqlCommandFactory();
            var command = (factory.Instantiate(client, query).Implementation) as SqlCommandOperation;
            var statement = command.Create();

            var clientOperation = client.CreateClientOperation();
            var results = clientOperation.Run(statement);
            Assert.That(results.Count, Is.EqualTo(4));
        }
    }
}
