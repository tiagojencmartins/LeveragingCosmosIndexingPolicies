using CosmosDB.Models.Abstract;
using Newtonsoft.Json;

namespace CosmosDB.Models
{
    public class Article : IDocumentBase
    {
        [JsonProperty("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Category { get; }

        public string Title { get; set; } = new string('*', Random.Shared.Next(40, 60));

        public string Description { get; }

        public int CharacterCount { get; } = 0;

        private Article()
        {
            var categories = new string[]
            {
                "Harry Potter And The Philosopher's Stone",
                "Harry Potter And Chamber of Secrets",
                "The Hunger Games"
            };

            Description = new string('*', Random.Shared.Next(1500, 10000));
            CharacterCount = Description.Length;
            Category = categories[Random.Shared.Next(0, categories.Length)];
        }

        public static Article Create()
        {
            return new Article();
        }
    }
}
