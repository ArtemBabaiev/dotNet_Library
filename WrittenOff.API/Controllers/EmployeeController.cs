using Microsoft.AspNetCore.Mvc;
using MediatR;
using AutoMapper;
using WrittenOffManagement.Application.DTO.Request;
using WrittenOffManagement.Application.CQRS.Query;
using WrittenOffManagement.Application.DTO.Response;
using WrittenOffManagement.Application.CQRS.Command;
using WrittenOffManagement.Domain.Entities;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WrittenOffManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ILogger<EmployeeController> logger;

        public EmployeeController(IMapper mapper, IMediator mediator, ILogger<EmployeeController> logger)
        {
            this.mapper = mapper;
            this.mediator = mediator;
            this.logger = logger;
        }

        // GET: api/<EmployeeController>
        [HttpGet(Name = "GetAllEmployees")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmployeeResponse>>> Get()
        {
            try
            {
                var result = (await mediator.Send(new GetAllEmployeesQuery())).Select(wo => mapper.Map<EmployeeResponse>(wo));
                logger.LogInformation($"Returned all writtenOffs from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Transaction Failed! Something went wrong inside GetAsync() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}", Name = "GetEmployeeById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<EmployeeResponse>> Get(long id)
        {
            try
            {
                var result = mapper.Map<EmployeeResponse>(
                    await mediator.Send(new GetEmployeeByIdQuery { Id = id })
                    );
                if (result == null)
                {
                    logger.LogError($"Employee with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.LogInformation($"Returned writtenOff with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside GetByIdAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST api/<EmployeeController>
        [HttpPost(Name = "CreateEmployee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Post([FromBody] EmployeeRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Employee object sent from client is null.");
                    return BadRequest("Employee object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Employee object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await mediator.Send(new CreateEmployeeCommand
                {
                    Employee = mapper.Map<Employee>(request)
                });
                logger.LogError("Created Employee object in DB.");
                return Ok();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside InsertAsync action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}", Name = "UpdateEmployeeWithId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put(int id, [FromBody] EmployeeRequest request)
        {
            try
            {
                if (request == null)
                {
                    logger.LogError("Employee object sent from client is null.");
                    return BadRequest("Employee object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.LogError("Invalid Employee object sent from client.");
                    return BadRequest("Invalid Employee object");
                }
                request.Id = id;
                await mediator.Send(new UpdateEmployeeCommand
                {
                    Employee = mapper.Map<Employee>(request),
                    Id = id
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                logger.LogError($"Something went wrong inside UpdateAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}", Name = "DeleteEmployeeById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await mediator.Send(new DeleteEmployeeCommand
                {
                    Id = id
                });
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
