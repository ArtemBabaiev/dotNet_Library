using Catalog.BLL.DTO.Request;
using Gateway.API.Services;
using Gateway.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.DTO.Response;
using WrittenOffManagement.Domain.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritingOffController : ControllerBase
    {

        /*IWrittenOffService writtenOffService;
        ICatalogService catalogService;

        public WritingOffController(IWrittenOffService writtenOffService, ICatalogService catalogService)
        {
            this.writtenOffService = writtenOffService;
            this.catalogService = catalogService;
        }


        // GET: api/<WritingOffController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WrittenOffResponse>>> Get()
        {
            return Ok(await writtenOffService.GetWrittenOffs());
        }

        // POST api/<WritingOffController>
        [HttpPost]
        public async void Post([FromBody] ExemplarRequest request)
        {
            await catalogService.deleteExemplar(request.Id);
            await writtenOffService.createWrittenOff(request, await catalogService.getById(request.LiteratureId));
        }*/

    }
}
