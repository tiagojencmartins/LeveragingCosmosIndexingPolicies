using CosmosDB.Models.Abstract;
using CosmosDB.Services.Abstract;
using Microsoft.Azure.Cosmos;
using System.Net;

namespace CosmosDB.Services
{
    public class CosmosService : ICosmosService
    {
        private const string DATABASE = "newspapper";
        private const string CONTAINER = "article";
        private const string PARTITION_KEY = "id";

        private readonly CosmosClient _client;

        public CosmosService(CosmosClient client)
        {
            _client = client;
        }

        public async Task<bool> CreateDatabaseAsync()
        {
            var result = await _client.CreateDatabaseIfNotExistsAsync(DATABASE);

            return result.StatusCode is HttpStatusCode.Created && result is not null;
        }

        public async Task<bool> CreateContainerAsync()
        {
            var database = _client.GetDatabase(DATABASE);

            if (database is null)
            {
                return false;
            }

            var container = await database.CreateContainerIfNotExistsAsync(CONTAINER, $"/{PARTITION_KEY}");

            return container.StatusCode is HttpStatusCode.Created && container is not null;
        }

        public async Task GetDataAsync<T>() where T : IDocumentBase
        {
            var container = _client.GetContainer(DATABASE, CONTAINER);

            string sqlQueryText = "SELECT TOP 100 * FROM c WHERE c.Title = '*********************************************************' ORDER BY c.CharacterCount DESC";

            var query = new QueryDefinition(sqlQueryText);

            FeedIterator<T> resultSetIterator = container.GetItemQueryIterator<T>(
                query, requestOptions: new QueryRequestOptions
                {
                    PopulateIndexMetrics = true
                });

            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                Console.WriteLine(response.IndexMetrics);
            }
        }

        public async Task<bool> SeedAsync<T>(List<T> values) where T : IDocumentBase
        {
            var container = _client.GetContainer(DATABASE, CONTAINER);

            List<Task> tasks = new(values.Count);

            foreach (T item in values)
            {
                tasks.Add(container.CreateItemAsync(item, new PartitionKey(item.Id.ToString()))
                    .ContinueWith(itemResponse =>
                    {
                        if (!itemResponse.IsCompletedSuccessfully)
                        {
                            AggregateException innerExceptions = itemResponse.Exception.Flatten();
                            if (innerExceptions.InnerExceptions.FirstOrDefault(innerEx => innerEx is CosmosException) is CosmosException cosmosException)
                            {
                                Console.WriteLine($"Received {cosmosException.StatusCode} ({cosmosException.Message}).");
                            }
                            else
                            {
                                Console.WriteLine($"Exception {innerExceptions.InnerExceptions.FirstOrDefault()}.");
                            }
                        }
                    }));
            }

            await Task.WhenAll(tasks);

            return true;
        }
    }
}
