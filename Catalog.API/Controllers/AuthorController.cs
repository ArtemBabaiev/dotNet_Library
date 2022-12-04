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
    public class AuthorController : ControllerBase
    {
        private readonly ILogger logger;
        private IAuthorService authorService;

        public AuthorController(ILogger logger, IAuthorService authorService)
        {
            this.logger = logger;
            this.authorService = authorService;
        }


        // GET: api/<AuthorController>
        [HttpGet(Name = "GetAllAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AuthorResponse>>> Get()
        {
            try
            {
                var result = await authorService.GetAsync();
                logger.Information($"Returned all authors from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside GetAsync() action");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<AuthorController>/5
        [HttpGet("{id}", Name = "GetAuthorById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AuthorResponse>> Get(long id)
        {
            try
            {
                var result = await authorService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.Error("Author with id: {id}, hasn't been found in db.", id);
                    return NotFound();
                }
                logger.Information("Returned author with id: {id}", id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Something went wrong inside GetByIdAsync action with {id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<AuthorController>
        [HttpPost(Name = "CreateAuthor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] AuthorRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Author object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await authorService.InsertAsync(request);
                logger.Information("Created Author object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<AuthorController>/5
        [HttpPut("{id}", Name = "UpdateAuthorWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] AuthorRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Author object sent from client.");
                    return BadRequest("Invalid Author object");
                }
                request.Id = id;
                await authorService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<AuthorController>/5
        [HttpDelete("{id}", Name = "DeleteAuthorById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await authorService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
