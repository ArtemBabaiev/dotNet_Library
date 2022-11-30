using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.CQRS.Query;
using WrittenOffManagement.Application.DTO.Response;
using WrittenOffManagement.Application.CQRS.Command;
using WrittenOffManagement.Domain.Entities;
using ILogger = Serilog.ILogger;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WrittenOffManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WrittenOffController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger logger;

        public WrittenOffController(IMapper mapper, IMediator mediator, ILogger logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        // GET: api/<WrittenOffController>
        [HttpGet(Name = "GetAllWrittenOffs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<WrittenOffResponse>>> Get()
        {
            try
            {
                var result = (await mediator.Send(new GetAllWrittenOffsQuery())).Select(wo => mapper.Map<WrittenOffResponse>(wo));
                logger.Information($"Returned all writtenOffs from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<WrittenOffController>/5
        [HttpGet("{id}", Name = "GetWrittenOffById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<WrittenOffResponse>> Get(long id)
        {
            try
            {
                var result = mapper.Map<WrittenOffResponse>(
                    await mediator.Send(new GetWrittenOffByIdQuery { Id = id })
                    );
                if (result == null)
                {
                    logger.Error($"WrittenOff with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned writtenOff with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<WrittenOffController>
        [HttpPost(Name = "CreateWrittenOff")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] WrittenOffRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("WrittenOff object sent from client is null.");
                    return BadRequest("WrittenOff object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid WrittenOff object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await mediator.Send(new CreateWrittenOffCommand
                {
                    WrittenOff = mapper.Map<WrittenOff>(request)
                });
                logger.Error("Created WrittenOff object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<WrittenOffController>/5
        [HttpPut("{id}", Name = "UpdateWrittenOffWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] WrittenOffRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.Error("WrittenOff object sent from client is null.");
                    return BadRequest("WrittenOff object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid WrittenOff object sent from client.");
                    return BadRequest("Invalid WrittenOff object");
                }
                request.Id = id;
                await mediator.Send(new UpdateWrittenOffCommand
                {
                    WrittenOff = mapper.Map<WrittenOff>(request),
                    Id = id
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<WrittenOffController>/5
        [HttpDelete("{id}", Name = "DeleteWrittenOffById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await mediator.Send(new DeleteWrittenOffCommand
                {
                    Id = id
                });
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
