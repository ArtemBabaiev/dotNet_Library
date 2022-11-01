
using Aggregator.DTO;
using Agregator.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aggregator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteratureController : ControllerBase
    {
        ICatalogService catalogService;
        IRecordMngmtService recordService;

        public LiteratureController(ICatalogService catalogService, IRecordMngmtService recordService)
        {
            this.catalogService = catalogService;
            this.recordService = recordService;
        }

        // POST api/<LiteratureController>
        [HttpPost]
        public async void Post([FromBody] LiteratureAggDTO request)
        {
            await catalogService.createLiterature(request);
            await recordService.createLiterature(request);
        }

        // PUT api/<LiteratureController>/5
        [HttpPut("{id}")]
        public async void Put(int id, [FromBody] LiteratureAggDTO request)
        {
            await catalogService.updateLiterature(id, request);
            await recordService.updateLiterature(id, request);
        }

        // DELETE api/<LiteratureController>/5
        [HttpDelete("{id}")]
        public async void Delete(long id)
        {
            await catalogService.deleteLiterature(id);
            await recordService.deleteLiterature(id);
        }
    }
}
