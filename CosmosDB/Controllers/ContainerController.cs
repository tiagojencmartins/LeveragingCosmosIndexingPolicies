using CosmosDB.Controllers.Abstract;
using CosmosDB.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDB.Controllers
{
    public class ContainerController : BaseController
    {
        public ContainerController(ICosmosService cosmosService) : base(cosmosService)
        {
        }

        [HttpGet("create")]
        public async Task<IActionResult> CreateContainer()
        {
            var response = await _cosmosService.CreateContainerAsync();

            if (!response)
            {
                return BadRequest();
            }

            return Ok("Container created");
        }
    }
}
