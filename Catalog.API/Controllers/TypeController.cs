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
    public class TypeController : ControllerBase
    {
        private readonly ILogger logger;
        private ITypeService typeService;

        public TypeController(ILogger logger, ITypeService typeService)
        {
            this.logger = logger;
            this.typeService = typeService;
        }


        // GET: api/<TypeController>
        [HttpGet(Name = "GetAllTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<TypeResponse>>> Get()
        {
            try
            {
                var result = await typeService.GetAsync();
                logger.Information($"Returned all types from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<TypeController>/5
        [HttpGet("{id}", Name = "GetTypeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TypeResponse>> Get(long id)
        {
            try
            {
                var result = await typeService.GetByIdAsync(id);
                if (result == null)
                {
                    logger.Error($"Type with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned type with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<TypeController>
        [HttpPost(Name = "CreateType")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] TypeRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Type object sent from client is null.");
                    return BadRequest("Type object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Type object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await typeService.InsertAsync(request);
                logger.Error("Created Type object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<TypeController>/5
        [HttpPut("{id}", Name = "UpdateTypeWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] TypeRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("Type object sent from client is null.");
                    return BadRequest("Type object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Type object sent from client.");
                    return BadRequest("Invalid Type object");
                }
                request.Id = id;
                await typeService.UpdateAsync(request);

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<TypeController>/5
        [HttpDelete("{id}", Name = "DeleteTypeById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await typeService.DeleteAsync(id);
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
