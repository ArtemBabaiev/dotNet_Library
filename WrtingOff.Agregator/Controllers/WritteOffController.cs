
using Catalog.BLL.DTO.Request;
using Microsoft.AspNetCore.Mvc;
using WrittenOffManagement.Application.DTO.Response;
using Agregator.Services.Interfaces;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Agregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritingOffController : ControllerBase
    {

        IWrittenOffService writtenOffService;
        ICatalogService catalogService;
        IRecordMngmtService recordManagment;

        public WritingOffController(IWrittenOffService writtenOffService, ICatalogService catalogService, IRecordMngmtService recordManagment)
        {
            this.writtenOffService = writtenOffService;
            this.catalogService = catalogService;
            this.recordManagment = recordManagment;
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
            await recordManagment.deleteExemplar(request.Id);
            await writtenOffService.createWrittenOff(request, await catalogService.getLiteratureById(request.LiteratureId));
        }

    }
}
