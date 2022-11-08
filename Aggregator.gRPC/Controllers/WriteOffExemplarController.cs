using Aggregator.gRPC.Services;
using Microsoft.AspNetCore.Mvc;
using WrittenOffManagement.API.Protos;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aggregator.gRPC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WriteOffExemplarController : ControllerBase
    {
        ExemplarAggregatorService exemplarAggregatorService;

        public WriteOffExemplarController(ExemplarAggregatorService exemplarAggregatorService)
        {
            this.exemplarAggregatorService = exemplarAggregatorService;
        }


        // DELETE api/<WriteOffExemplarController>/5
        [HttpDelete("{id}")]
        public async Task<WrittenOffModel> Delete(long id)
        {
            return await exemplarAggregatorService.writeOfExemplar(id);
        }
    }
}
