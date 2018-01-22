using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.CosmosDb.Query.Client
{
    abstract class BaseClient : NBi.Core.Query.Client.IClient
    {
        public const string EndpointToken = "endpoint";
        public const string AuthKeyToken = "authkey";
        public const string DatabaseToken = "database";
        public const string CollectionToken = "collection";
        public const string ApiToken = "api";

        private readonly Uri endpoint;

        protected string AuthKey { get => (string)ConnectionStringTokens[AuthKeyToken]; }
        protected string Endpoint { get => (string)ConnectionStringTokens[EndpointToken]; }
        protected string DatabaseId { get => (string)ConnectionStringTokens[DatabaseToken]; }
        protected string CollectionId { get => (string)ConnectionStringTokens[CollectionToken]; }
        protected DbConnectionStringBuilder ConnectionStringTokens => new DbConnectionStringBuilder() { ConnectionString = ConnectionString };

        public string ConnectionString { get; }
        public abstract Type UnderlyingSessionType { get; }
        public abstract object CreateNew();
        
        internal BaseClient(Uri endpoint, string authkey, string databaseId, string collectionId, string api)
        {
            this.endpoint = endpoint;

            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                { EndpointToken, endpoint.ToString() },
                { AuthKeyToken, authkey },
                { DatabaseToken, databaseId },
                { CollectionToken, collectionId },
                { ApiToken, api}
            };
            ConnectionString = connectionStringBuilder.ToString();
        }
    }
}
