using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WritingController : ControllerBase
    {
        private readonly ILogger<WritingController> logger;
        private IWritingService writingService;

        public WritingController(ILogger<WritingController> logger, IWritingService writingService)
        {
            this.logger = logger;
            this.writingService = writingService;
        }


        // GET: api/<WritingController>
        [HttpGet(Name = "GetAllWritings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<WritingResponse>>> Get()
        {
            try
            {
                var result = await writingService.GetAsync();
                logger.LogInformation($"Returned all writings from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<WritingController>/5
        [HttpGet("{id}", Name = "GetWritingById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WritingResponse>> Get(long id)
        {
            try
            {
                var result = await writingService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.LogError($"Writing with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned writing with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<WritingController>
        [HttpPost(Name = "CreateWriting")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] WritingRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Writing object sent from client is null.");
                    return BadRequest("Writing object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Writing object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await writingService.InsertAsync(request);
                logger.LogError("Created Writing object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<WritingController>/5
        [HttpPut("{id}", Name = "UpdateWritingWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] WritingRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Writing object sent from client is null.");
                    return BadRequest("Writing object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Writing object sent from client.");
                    return BadRequest("Invalid Writing object");
                }
                request.Id = id;
                await writingService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<WritingController>/5
        [HttpDelete("{id}", Name = "DeleteWritingById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await writingService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("author/{authorId}", Name = "GetWritingsWithAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<WritingResponse>>> GetWithAuthor(long authorId)
        {
            try
            {
                var result = await writingService.GetAllWritingsWithAuthor(authorId);
                if (result == null)
                {
                    logger.LogError($"Writings with author with id: {authorId}, hasn't been found in db.");
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
