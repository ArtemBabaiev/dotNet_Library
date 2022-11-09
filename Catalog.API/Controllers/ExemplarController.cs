using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Catalog.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemplarController : ControllerBase
    {
        private readonly ILogger<ExemplarController> logger;
        private IExemplarService exemplarService;
        private IMemoryCache cache;

        public ExemplarController(ILogger<ExemplarController> logger, IExemplarService exemplarService, IMemoryCache cache)
        {
            this.logger = logger;
            this.exemplarService = exemplarService;
            this.cache = cache;
        }




        // GET: api/<ExemplarController>
        [HttpGet(Name = "GetAllExemplars")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ExemplarResponse>>> Get()
        {
            try
            {
                if (cache.TryGetValue("ExemplarList", out IEnumerable<ExemplarResponse> exemplars))
                {
                    logger.Log(LogLevel.Information, "Exemplar list found in cache.");
                }
                else
                {
                    logger.LogInformation("Exemplar list not found in cache. Fetching from database.");
                    exemplars = await exemplarService.GetAsync();
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                            .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                            .SetPriority(CacheItemPriority.Normal)
                            .SetSize(1024);
                    cache.Set("ExemplarList", exemplars, cacheEntryOptions);
                }
                return Ok(exemplars);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<ExemplarController>/5
        [HttpGet("{id}", Name = "GetExemplarById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExemplarResponse>> Get(long id)
        {
            try
            {
                var result = await exemplarService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.LogError($"Exemplar with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned exemplar with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<ExemplarController>
        [HttpPost(Name = "CreateExemplar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] ExemplarRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Exemplar object sent from client is null.");
                    return BadRequest("Exemplar object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Exemplar object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await exemplarService.InsertAsync(request);
                logger.LogError("Created Exemplar object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<ExemplarController>/5
        [HttpPut("{id}", Name = "UpdateExemplarWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] ExemplarRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Exemplar object sent from client is null.");
                    return BadRequest("Exemplar object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Exemplar object sent from client.");
                    return BadRequest("Invalid Exemplar object");
                }
                request.Id = id;
                await exemplarService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<ExemplarController>/5
        [HttpDelete("{id}", Name = "DeleteExemplarById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await exemplarService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<ExemplarController>/5
        [HttpDelete("rabbit/{id}", Name = "DeleteExemplarByIdRabbitMQ")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteRabbit(long id)
        {
            try
            {
                await exemplarService.DeleteUsingRabbitMQ(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
