using NBi.Core;
using NBi.Core.CosmosDb.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Unit.Query.Client
{
    public class SqlClientTest
    {
        private string base64AuthKey = Convert.ToBase64String(Encoding.UTF8.GetBytes("@uthK3y"));

        [Test]
        public void InstantiateUnderlyingSession_CosmosDbConnectionString_ISession()
        {
            var factory = new SqlClientFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey={base64AuthKey};database=db;collection=FoF;api=sql");
            Assert.That(session.UnderlyingSessionType, Is.EqualTo(typeof(SqlClientOperation)));
        }

        [Test]
        public void InstantiateCreate_CosmosDbConnectionString_ISession()
        {
            var factory = new SqlClientFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey={base64AuthKey};database=db;collection=FoF;api=sql");
            var underlyingSession = session.CreateNew();
            Assert.That(underlyingSession, Is.Not.Null);
            Assert.That(underlyingSession, Is.AssignableTo<SqlClientOperation>());
        }

        [Test]
        public void InstantiateCreate_CosmosDbConnectionStringWithoutBase64Encoding_NBiException()
        {
            var factory = new SqlClientFactory();
            var session = factory.Instantiate($"Endpoint=https://xyz.graphs.azure.com:443;AuthKey=@uthK3y;database=db;collection=FoF;api=sql");
            Assert.Throws<NBiException>( () => session.CreateNew());
        }
    }
}
