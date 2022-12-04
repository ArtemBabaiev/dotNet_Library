using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly ILogger logger;
        private IPublisherService publisherService;

        public PublisherController(ILogger logger, IPublisherService publisherService)
        {
            this.logger = logger;
            this.publisherService = publisherService;
        }


        // GET: api/<PublisherController>
        [HttpGet(Name = "GetAllPublishers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PublisherResponse>>> Get()
        {
            try
            {
                var result = await publisherService.GetAsync();
                logger.Information($"Returned all publishers from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<PublisherController>/5
        [HttpGet("{id}", Name = "GetPublisherById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PublisherResponse>> Get(long id)
        {
            try
            {
                var result = await publisherService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.Error($"Publisher with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned publisher with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<PublisherController>
        [HttpPost(Name = "CreatePublisher")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] PublisherRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Publisher object sent from client is null.");
                    return BadRequest("Publisher object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Publisher object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await publisherService.InsertAsync(request);
                logger.Error("Created Publisher object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<PublisherController>/5
        [HttpPut("{id}", Name = "UpdatePublisherWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] PublisherRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Publisher object sent from client is null.");
                    return BadRequest("Publisher object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Publisher object sent from client.");
                    return BadRequest("Invalid Publisher object");
                }
                request.Id = id;
                await publisherService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<PublisherController>/5
        [HttpDelete("{id}", Name = "DeletePublisherById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await publisherService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
