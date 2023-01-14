using CosmosDB.Controllers.Abstract;
using CosmosDB.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDB.Controllers
{
    public class DatabaseController : BaseController
    {
        public DatabaseController(ICosmosService cosmosService) : base(cosmosService)
        {
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateDatabase()
        {
            var response = await _cosmosService.CreateDatabaseAsync();

            if (!response)
            {
                return BadRequest();
            }

            return Ok("Database created");
        }
    }
}
