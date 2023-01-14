using CosmosDB.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace CosmosDB.Controllers.Abstract
{
    [ApiController, Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected readonly ICosmosService _cosmosService;

        public BaseController(ICosmosService cosmosService)
        {
            _cosmosService = cosmosService;
        }
    }
}
