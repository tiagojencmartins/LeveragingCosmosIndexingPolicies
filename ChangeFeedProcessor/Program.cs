using ChangeFeedProcessorExample.Models;
using Microsoft.Azure.Cosmos;

namespace ChangeFeedProcessorExample
{

    class Program
    {
        private static CosmosClient _cosmosClient;

        private static Database _productDatabase;
        private static Container _productContainer;
        private static Container _productLeaseContainer;

        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting Change Feed Sample");

            _cosmosClient = new CosmosClient("AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
            _productDatabase = _cosmosClient.GetDatabase("library");
            _productContainer = _productDatabase.GetContainer("books");
            _productLeaseContainer = await _productDatabase.CreateContainerIfNotExistsAsync("lease1", "/id");

            try
            {
                await StartChangeFeedProcessorAsync(_cosmosClient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong. Exception thrown: {ex.Message}");
                throw;
            }
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static async Task<ChangeFeedProcessor> StartChangeFeedProcessorAsync(
            CosmosClient cosmosClient)
        {
            Container leaseContainer = cosmosClient.GetContainer(_productDatabase.Id, _productLeaseContainer.Id);
            ChangeFeedProcessor changeFeedProcessor = cosmosClient.GetContainer(_productDatabase.Id, _productContainer.Id)
                .GetChangeFeedProcessorBuilder<BookModel>("cosmosBackupSample", HandleChangesAsync)
                .WithInstanceName("consoleHost")
                .WithLeaseContainer(leaseContainer)
                .Build();

            Console.WriteLine("Starting Change Feed Processor for Product Backups...");
            await changeFeedProcessor.StartAsync();
            Console.WriteLine("Product Backup started");
            return changeFeedProcessor;
        }

        private static async Task HandleChangesAsync(
            IReadOnlyCollection<BookModel> changes,
            CancellationToken cancellationToken)
        {
            Console.WriteLine("Backing up Product items");

            foreach (BookModel product in changes)
            {
                //await _productBackUpContainer.CreateItemAsync(
                //product,
                //    partitionKey: new PartitionKey(product.ProductCategory));
                //Console.WriteLine($"Product added to backup container");
                //await Task.Delay(10);
            }

            Console.WriteLine("Finished handling changes");
        }
    }
}