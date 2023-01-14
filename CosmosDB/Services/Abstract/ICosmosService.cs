using CosmosDB.Models.Abstract;

namespace CosmosDB.Services.Abstract
{
    public interface ICosmosService
    {
        Task<bool> CreateDatabaseAsync();

        Task<bool> CreateContainerAsync();

        Task<bool> SeedAsync<T>(List<T> values) where T : IDocumentBase;

        Task GetDataAsync<T>() where T : IDocumentBase;
    }
}
