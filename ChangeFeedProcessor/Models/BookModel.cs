using Newtonsoft.Json;

namespace ChangeFeedProcessorExample.Models
{
    public class BookModel
    {
        [JsonProperty("id")]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ISBN { get; set; } = Guid.NewGuid().ToString();

        public string Author { get; set; } = Guid.NewGuid().ToString();

        public int NumberOfPages { get; set; } = Random.Shared.Next(20, 1000);

        public string Name { get; set; }

        public string Genre { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; } = Random.Shared.Next(1999, 2001).ToString();
    }
}
