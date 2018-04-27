using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Graphs;
using Microsoft.Azure.Graphs.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    abstract class BaseClientOperation
    {
        public DocumentClient Client { get; }
        public string DatabaseId { get; }
        public string CollectionId { get; }
        protected DocumentCollection Collection { get; private set; }

        public BaseClientOperation(Uri endpoint, string authKey, string databaseId, string graphId)
        {
            try
            { Client = new DocumentClient(endpoint, authKey); }
            catch (FormatException ex)
            { throw new Exception($"The connectionString for CosmosDb is expecting an AuthKey encoded in base64. The value '{authKey}' is not a base64-encoded.", ex); }

            DatabaseId = databaseId ?? throw new Exception($"The connectionString for CosmosDb is expecting a databaseId and this value cannot be null or empty");
            CollectionId = graphId ?? throw new Exception($"The connectionString for CosmosDb is expecting a collectionId and this value cannot be null or empty");
        }


        public dynamic[] Run(IDocumentQuery<dynamic> query)
        {
            var result = GetAllResultsAsync(query).Result;
            return result;
        }

        private async static Task<T[]> GetAllResultsAsync<T>(IDocumentQuery<T> queryAll)
        {
            var list = new List<T>();
            while (queryAll.HasMoreResults)
            {
                var docs = await queryAll.ExecuteNextAsync<T>();
                foreach (var d in docs)
                    list.Add(d);
            }
            return list.ToArray();
        }

        public IDocumentQuery<dynamic> CreateCommand(string preparedStatement)
        {
            Initialize();
            return OnCreateCommand(preparedStatement);
        }

        protected abstract IDocumentQuery<dynamic> OnCreateCommand(string preparedStatement);

        protected void Initialize()
        {
            var db = GetDatabaseAsync(Client, DatabaseId).Result;
            Collection = GetCollectionAsync(Client, DatabaseId, CollectionId).Result;
        }

        private async Task<Database> GetDatabaseAsync(DocumentClient client, string databaseid)
        {
            try
            {
                return await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.StatusCode == System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"The database '{DatabaseId}' does not exist.");
                else
                    throw;
            }
            throw new InvalidOperationException();
        }

        private async Task<DocumentCollection> GetCollectionAsync(DocumentClient client, string databaseId, string collectionId)
        {
            try
            {
                return await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch (DocumentClientException documentClientException)
            {
                if (documentClientException.Error?.Code == "NotFound")
                {
                    if (documentClientException.Error?.Code == "NotFound")
                        throw new Exception($"The collection '{collectionId}' does not exist.");
                    else
                        throw;
                }
            }
            throw new InvalidOperationException();
        }
    }
}
