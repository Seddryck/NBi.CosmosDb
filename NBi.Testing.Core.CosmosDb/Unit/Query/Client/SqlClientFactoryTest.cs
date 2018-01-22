using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.CosmosDb.Query.Client;

namespace NBi.Testing.Core.CosmosDb.Unit.Query.Client
{
    public class SqlClientFactoryTest
    {
        [Test]
        public void CanHandle_CosmosDbSql_True()
        {
            var factory = new SqlClientFactory();
            Assert.That(factory.CanHandle("Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthk3y;database=db;collection=FoF;api=sql"), Is.True);
        }

        [Test]
        public void CanHandle_OleDbConnectionString_False()
        {
            var factory = new SqlClientFactory();
            Assert.That(factory.CanHandle("data source=SERVER;initial catalog=DB;IntegratedSecurity=true;Provider=OLEDB.1"), Is.False);
        }

        [Test]
        public void Instantiate_CosmosDbSql_SqlClient()
        {
            var factory = new SqlClientFactory();
            var session = factory.Instantiate("Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthk3y;database=db;collection=FoF;api=sql");
            Assert.That(session, Is.Not.Null);
            Assert.That(session, Is.TypeOf<SqlClient>());
        }

        
    }
}
