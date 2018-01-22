using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Integration
{
    class NativeApi
    {
        [Test]
        public void SqlApi()
        {
            var client = new DocumentClient(new Uri("https://localhost:8081/"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri("NBi.GraphDB", "FoF")).Result.Resource;
            var queryable = client.CreateDocumentQuery<dynamic>(collection.SelfLink, "SELECT VALUE p.age[0]._value FROM ROOT p WHERE p.label = 'person' and p.firstName[0]._value = 'Mary'");
            var query = queryable.AsDocumentQuery();
            Assert.That(query.HasMoreResults);
            var res = query.ExecuteNextAsync().Result;
            Assert.IsInstanceOf<JValue>(res.FirstOrDefault());
            Assert.That(res.FirstOrDefault().Value, Is.EqualTo(39));
        }

        [Test]
        public void GrapApi()
        {
            var client = new DocumentClient(new Uri("https://localhost:8081/"), "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            var collection = client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri("NBi.GraphDB", "FoF")).Result.Resource;
            var query = client.CreateGremlinQuery<dynamic>(collection, "g.V().has('firstName', 'Mary').values('age').Limit(1)");
            Assert.That(query.HasMoreResults);
            var res = query.ExecuteNextAsync().Result;
            Assert.IsInstanceOf<Int64>(res.FirstOrDefault());
            Assert.That(res.FirstOrDefault(), Is.EqualTo(39));
        }
    }
}
