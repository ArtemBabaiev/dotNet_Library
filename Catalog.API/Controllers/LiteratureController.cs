using Catalog.API.Cache;
using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LiteratureController : ControllerBase
    {
        private readonly ILogger<LiteratureController> logger;
        private ILiteratureService literatureService;
        IDistributedCache cache;

        public LiteratureController(ILogger<LiteratureController> logger, ILiteratureService literatureService, IDistributedCache cache)
        {
            this.logger = logger;
            this.literatureService = literatureService;
            this.cache = cache;
        }




        // GET: api/<LiteratureController>
        [HttpGet(Name = "GetAllLiteratures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LiteratureResponse>>> Get()
        {
            try
            {
                IEnumerable<LiteratureResponse>? literature;
                string recordKey = $"Literature_{DateTime.Now:yyyyMMdd_hhmm}";
                literature = await cache.GetRecordAsync<IEnumerable<LiteratureResponse>>(recordKey);
                logger.LogInformation($"Trying to load from cache");
                if (literature == null)
                {
                    logger.LogInformation($"Loading from database");
                    literature = await literatureService.GetAsync();
                    await cache.SetRecordAsync(recordKey, literature, null, TimeSpan.FromSeconds(60));
                }

                logger.LogInformation($"Returned all literatures.");
                return Ok(literature);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<LiteratureController>/5
        [HttpGet("{id}", Name = "GetLiteratureById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LiteratureResponse>> Get(long id)
        {
            try
            {
                var result = await literatureService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.LogError($"Literature with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned literature with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<LiteratureController>
        [HttpPost(Name = "CreateLiterature")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] LiteratureRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Literature object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await literatureService.InsertAsync(request);
                logger.LogError("Created Literature object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<LiteratureController>/5
        [HttpPut("{id}", Name = "UpdateLiteratureWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] LiteratureRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Literature object sent from client.");
                    return BadRequest("Invalid Literature object");
                }
                request.Id = id;
                await literatureService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<LiteratureController>/5
        [HttpDelete("{id}", Name = "DeleteLiteratureById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await literatureService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }




        [HttpGet("author/{authorId}", Name = "GetLiteratureByAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LiteratureResponse>>> GetWithAuthor(long authorId)
        {
            try
            {
                var result = await literatureService.GetAllWithAuthor(authorId);
                if (result == null)
                {
                    logger.LogError($"Literature with author with id: {authorId}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned literature with author with id: {authorId}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetWithAuthor action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
