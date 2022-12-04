using Microsoft.AspNetCore.Mvc;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.Service.Interface;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecordManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger logger;
        private IEmployeeService employeeService;

        public EmployeeController(ILogger logger, IEmployeeService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        // GET: api/<EmployeeController>
        [HttpGet(Name = "GetAllEmployees")]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> Get()
        {
            try
            {
                var result = await employeeService.GetAllEmployees();
                logger.Information($"Returned all employees from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}", Name ="GetEmployeeById")]
        public async Task<ActionResult<EmployeeDTO>> Get(long id)
        {
            try
            {
                var result = await employeeService.GetEmployeeById(id);
                if (result == null)
                {
                    logger.Error($"Employee with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                logger.Information($"Returned employee with id: {id}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside GetAsync action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
            
        }

        // POST api/<EmployeeController>
        [HttpPost(Name = "CreateEmployee")]
        public async Task<ActionResult> Post([FromBody] EmployeeDTO newEmployeeDTO)
        {
            try
            {
                if (newEmployeeDTO == null)
                {
                    logger.Error("Employee object sent from client is null.");
                    return BadRequest("Employee object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Employee object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdEmployeeDTO = await employeeService.CreateEmployee(newEmployeeDTO);
                return CreatedAtRoute("GetEmployeeById", new { id = createdEmployeeDTO.Id }, createdEmployeeDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside POST action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}", Name = "UpdateEmployee")]
        public async Task<ActionResult> Put(long id, [FromBody] EmployeeDTO updateEmployeeDTO)
        {
            try
            {
                if (updateEmployeeDTO == null)
                {
                    logger.Error("Employee object sent from client is null.");
                    return BadRequest("Employee object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Employee object sent from client.");
                    return BadRequest("Invalid Employee object");
                }
                updateEmployeeDTO.Id = id;
                EmployeeDTO employeeDTO = await employeeService.UpdateEmployee(updateEmployeeDTO);
                if (employeeDTO == null)
                {
                    logger.Error($"Employee with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside Put action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}", Name = "DeleteEmployee")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await employeeService.DeleteEmployee(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside Delete action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
