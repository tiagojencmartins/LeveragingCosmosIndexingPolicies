using CosmosDB.Controllers.Abstract;
using CosmosDB.Models;
using CosmosDB.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDB.Controllers
{
    public class DocumentController : BaseController
    {
        public DocumentController(ICosmosService cosmosService) : base(cosmosService)
        {
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Seed()
        {
            var response = await _cosmosService.SeedAsync(Enumerable.Range(0, 3000).Select(_ => Article.Create()).ToList());

            if (!response)
            {
                return BadRequest();
            }

            return Ok($"Seed completed");
        }

        [HttpGet("top")]
        public async Task<IActionResult> ListTopArticles()
        {
            await _cosmosService.GetDataAsync<Article>();

            return Ok();
        }
    }
}
