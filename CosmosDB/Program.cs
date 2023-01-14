using CosmosDB.Services;
using CosmosDB.Services.Abstract;
using Microsoft.Azure.Cosmos;

namespace CosmosDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddSingleton(_ =>
            new CosmosClient(
                builder.Configuration.GetConnectionString("CosmosDB"),
                new CosmosClientOptions()
                {
                    SerializerOptions = new()
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.Default,
                        IgnoreNullValues = true
                    },
                    AllowBulkExecution = true
                }));

            builder.Services.AddSingleton<ICosmosService, CosmosService>();


            builder.Services.AddControllers();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}