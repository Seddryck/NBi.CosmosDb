using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data;
using NBi.Core.CosmosDb.Query.Client;
using NBi.Core.CosmosDb.Query.Command;
using NBi.Core.CosmosDb.Query.Execution;
using Moq;
using NBi.Extensibility.Query;

namespace NBi.Testing.Core.CosmosDb.Integration.Query.Execution
{
    [TestFixture]
    public class SqlExecutionEngineTest
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
        public void Execute_Vertex_DataSetFilled()
        {
            SqlClient client = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT * FROM FoF f WHERE f.label='person'");
            SqlCommandOperation commandOperation = new SqlCommandFactory().Instantiate(client, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(client.CreateClientOperation(), commandOperation);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(4));

            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("id"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("label"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("firstName"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("age"));
        }

        [Test]
        public void Execute_Edge_DataSetFilled()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT * FROM FoF f WHERE f.label='knows'");
            SqlCommandOperation commandOperation = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), commandOperation);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(3));

            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(12));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("id"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("label"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("_sink"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("_vertexId"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("_sinkLabel"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("_vertexLabel"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Assert.That(row["id"], Is.Not.Null.Or.Empty);
                Assert.That(row["_vertexLabel"], Is.EqualTo("person"));
                Assert.That(row["_sinkLabel"], Is.EqualTo("person"));
                Assert.That(row["_isEdge"], Is.True);
            }
        }

        [Test]
        public void Execute_ProjectionOfObjects_DataSetFilled()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT p.firstName[0]._value as FirstName, p.age[0]._value as Age FROM ROOT p WHERE p.label = 'person'");
            SqlCommandOperation commandOperation = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), commandOperation);

            var ds = engine.Execute();
            Assert.That(ds.Tables, Has.Count.EqualTo(1));
            Assert.That(ds.Tables[0].Rows, Has.Count.EqualTo(4));

            Assert.That(ds.Tables[0].Columns, Has.Count.EqualTo(2));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("FirstName"));
            Assert.That(ds.Tables[0].Columns.Cast<DataColumn>().Select(x => x.ColumnName), Has.Member("Age"));

            var firstNames = new List<object>();
            var ages = new List<object>();
            foreach (DataRow row in ds.Tables[0].Rows)
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (row.Table.Columns[i].ColumnName == "FirstName")
                        firstNames.Add(row.ItemArray[i]);
                    else if (row.Table.Columns[i].ColumnName == "Age")
                        ages.Add(row.ItemArray[i]);
                }

            foreach (var expectedFirstName in new[] { "Thomas", "Mary", "Ben", "Robin" })
                Assert.That(firstNames, Has.Member(expectedFirstName));

            foreach (var expectedAge in new object[] { 44, 39 })
                Assert.That(ages, Has.Member(expectedAge));
        }

        [Test]
        public void Execute_Integer_ScalarReturned()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT VALUE count(1) FROM FoF f WHERE f.label='person'");
            SqlCommandOperation cosmosdbQuery = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), cosmosdbQuery);

            var age = engine.ExecuteScalar();
            Assert.That(age, Is.EqualTo(4));
        }

        [Test]
        public void Execute_String_ScalarReturned()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT VALUE p.lastName[0]._value FROM ROOT p WHERE p.label = 'person' and p.firstName[0]._value = 'Mary'");
            SqlCommandOperation cosmosdbQuery = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), cosmosdbQuery);

            var lastName = engine.ExecuteScalar();
            Assert.That(lastName, Is.EqualTo("Andersen"));
        }

        [Test]
        public void Execute_NullString_ScalarReturned()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT VALUE p.lastName[0]._value FROM ROOT p WHERE p.label = 'person' and p.firstName[0]._value = 'Thomas'");
            SqlCommandOperation cosmosdbQuery = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), cosmosdbQuery);

            var count = engine.ExecuteScalar();
            Assert.That(count, Is.Null);
        }

        [Test]
        public void Execute_ListOfString_ListReturned()
        {
            SqlClient session = new SqlClientFactory().Instantiate(ConnectionStringReader.GetLocaleSql()) as SqlClient;
            var statement = Mock.Of<IQuery>(x => x.Statement == "SELECT VALUE p.lastName[0]._value FROM ROOT p WHERE p.label = 'person'");
            SqlCommandOperation cosmosdbQuery = new SqlCommandFactory().Instantiate(session, statement).Implementation as SqlCommandOperation;

            var engine = new SqlExecutionEngine(session.CreateClientOperation(), cosmosdbQuery);

            var count = engine.ExecuteList<object>();
            Assert.That(count, Has.Member("Andersen"));
            Assert.That(count, Has.Member("Miller"));
            Assert.That(count, Has.Member("Wakefield"));
        }

    }
}
