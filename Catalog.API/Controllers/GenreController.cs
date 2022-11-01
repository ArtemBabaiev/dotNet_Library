using Catalog.BLL.DTO.Request;
using Catalog.BLL.DTO.Response;
using Catalog.BLL.Service.Interface;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catalog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenreController : ControllerBase
    {
        private readonly ILogger<GenreController> logger;
        private IGenreService genreService;

        public GenreController(ILogger<GenreController> logger, IGenreService genreService)
        {
            this.logger = logger;
            this.genreService = genreService;
        }


        // GET: api/<GenreController>
        [HttpGet(Name = "GetAllGenres")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GenreResponse>>> Get()
        {
            try
            {
                var result = await genreService.GetAsync();
                logger.LogInformation($"Returned all genres from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<GenreController>/5
        [HttpGet("{id}", Name = "GetGenreById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GenreResponse>> Get(long id)
        {
            try
            {
                var result = await genreService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.LogError($"Genre with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned genre with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<GenreController>
        [HttpPost(Name = "CreateGenre")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] GenreRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Genre object sent from client is null.");
                    return BadRequest("Genre object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Genre object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await genreService.InsertAsync(request);
                logger.LogError("Created Genre object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<GenreController>/5
        [HttpPut("{id}", Name = "UpdateGenreWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] GenreRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Genre object sent from client is null.");
                    return BadRequest("Genre object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Genre object sent from client.");
                    return BadRequest("Invalid Genre object");
                }
                request.Id = id;
                await genreService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<GenreController>/5
        [HttpDelete("{id}", Name = "DeleteGenreById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await genreService.DeleteAsync(id);
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
