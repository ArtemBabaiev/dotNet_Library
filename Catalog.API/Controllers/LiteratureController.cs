using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteratureController : ControllerBase
    {
        private readonly ILogger logger;
        private ILiteratureService literatureService;

        public LiteratureController(ILogger logger, ILiteratureService literatureService)
        {
            this.logger = logger;
            this.literatureService = literatureService;
        }


        // GET: api/<LiteratureController>
        [HttpGet(Name = "GetAllLiteratures")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LiteratureResponse>>> Get()
        {
            try
            {
                var result = await literatureService.GetAsync();
                logger.Information($"Returned all literatures from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
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
                    logger.Error($"Literature with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned literature with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetByIdAsync action: {ex.Message}");
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
                    logger.Error ("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Literature object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await literatureService.InsertAsync(request);
                logger.Error("Created Literature object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside InsertAsync action: {ex.Message}");
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
                    logger.Error("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Literature object sent from client.");
                    return BadRequest("Invalid Literature object");
                }
                request.Id = id;
                await literatureService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside UpdateAsync action: {ex.Message}");
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
                logger.Error(ex, $"Something went wrong inside DeleteAsync action: {ex.Message}");
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
                    logger.Error($"Literature with author with id: {authorId}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned literature with author with id: {authorId}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetWithAuthor action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
