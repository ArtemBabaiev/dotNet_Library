using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Catalog.DAL.Entity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> logger;
        private IAuthorService authorService;

        public AuthorController(ILogger<AuthorController> logger, IAuthorService authorService)
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
                logger.LogInformation($"Returned all authors from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
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
                    logger.LogError($"Author with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned author with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
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
                    logger.LogError("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Author object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await authorService.InsertAsync(request);
                logger.LogError("Created Author object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
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
                    logger.LogError("Author object sent from client is null.");
                    return BadRequest("Author object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Author object sent from client.");
                    return BadRequest("Invalid Author object");
                }
                request.Id = id;
                await authorService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
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
                logger.LogError($"Something went wrong inside DeleteAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
