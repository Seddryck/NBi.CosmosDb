using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Graphs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb
{
    [SetUpFixture]
    public class TestSuiteSetup
    {
        private string[] Statements
        {
            get => new[]
{
            "g.V().Drop()"
            , "g.addV('person').property('id', 'thomas').property('firstName', 'Thomas').property('age', 44)"
            , "g.addV('person').property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39)"
            , "g.addV('person').property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller')"
            , "g.addV('person').property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield')"
            , "g.V('thomas').addE('knows').to(g.V('mary'))"
            , "g.V('thomas').addE('knows').to(g.V('ben'))"
            , "g.V('ben').addE('knows').to(g.V('robin'))"
        };
        }

        [SetUp]
        public void Init()
        {
            var csBuilder = new DbConnectionStringBuilder() { ConnectionString = ConnectionStringReader.GetLocaleGraph() };
            var endpoint = new Uri(csBuilder["endpoint"].ToString());
            var authKey = csBuilder["authkey"].ToString();
            var databaseId = csBuilder["database"].ToString();
            var collectionId = csBuilder["collection"].ToString();

            using (var client = new DocumentClient(endpoint, authKey))
            {
                var databaseResponse = client.CreateDatabaseIfNotExistsAsync(new Database() { Id = databaseId }).Result;
                switch (databaseResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine($"Database {databaseId} already exists.");
                        break;
                    case System.Net.HttpStatusCode.Created:
                        Console.WriteLine($"Database {databaseId} created.");
                        break;
                    default:
                        throw new ArgumentException($"Can't create database {databaseId}: {databaseResponse.StatusCode}");
                }

                var databaseUri = UriFactory.CreateDatabaseUri(databaseId);
                var collectionResponse = client.CreateDocumentCollectionIfNotExistsAsync(databaseUri, new DocumentCollection() { Id = collectionId }).Result;
                switch (collectionResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine($"Collection {collectionId} already exists.");
                        break;
                    case System.Net.HttpStatusCode.Created:
                        Console.WriteLine($"Database {collectionId} created.");
                        break;
                    default:
                        throw new ArgumentException($"Can't create database {collectionId}: {collectionResponse.StatusCode}");
                }

                var queryCheck = client.CreateGremlinQuery<dynamic>(collectionResponse, "g.V().Count()");
                var count = queryCheck.ExecuteNextAsync().Result.First();

                if (count != 4)
                {
                    foreach (var statement in Statements)
                    {
                        var query = client.CreateGremlinQuery<dynamic>(collectionResponse, statement);
                        var feed = query.ExecuteNextAsync().Result;
                        Console.WriteLine($"Setup database: { statement }");
                    }
                } 
            }


        }
    }
}
