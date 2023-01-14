using Newtonsoft.Json;

namespace CosmosDB.Models.Abstract
{
    public interface IDocumentBase
    {
        public Guid Id { get; set; }
    }
}
