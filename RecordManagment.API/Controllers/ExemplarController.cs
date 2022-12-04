using Microsoft.AspNetCore.Mvc;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.Service.Interface;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecordManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemplarController : ControllerBase
    {
        private readonly ILogger logger;
        private IExemplarService employeeService;

        public ExemplarController(ILogger logger, IExemplarService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        // GET: api/<ExemplarController>
        [HttpGet(Name = "GetAllExemplars")]
        public async Task<ActionResult<IEnumerable<ExemplarDTO>>> Get()
        {
            try
            {
                var result = await employeeService.GetAllExemplars();
                logger.Information($"Returned all employees from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<ExemplarController>/5
        [HttpGet("{id}", Name = "GetExemplarById")]
        public async Task<ActionResult<ExemplarDTO>> Get(long id)
        {
            try
            {
                var result = await employeeService.GetExemplarById(id);
                if (result == null)
                {
                    logger.Error($"Exemplar with id: {id}, hasn't been found in db.");
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

        // POST api/<ExemplarController>
        [HttpPost(Name = "CreateExemplar")]
        public async Task<ActionResult> Post([FromBody] ExemplarDTO newExemplarDTO)
        {
            try
            {
                if (newExemplarDTO == null)
                {
                    logger.Error("Exemplar object sent from client is null.");
                    return BadRequest("Exemplar object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Exemplar object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdExemplarDTO = await employeeService.CreateExemplar(newExemplarDTO);
                return CreatedAtRoute("GetExemplarById", new { id = createdExemplarDTO.Id }, createdExemplarDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside POST action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<ExemplarController>/5
        [HttpPut("{id}", Name = "UpdateExemplar")]
        public async Task<ActionResult> Put(long id, [FromBody] ExemplarDTO updateExemplarDTO)
        {
            try
            {
                if (updateExemplarDTO == null)
                {
                    logger.Error("Exemplar object sent from client is null.");
                    return BadRequest("Exemplar object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Exemplar object sent from client.");
                    return BadRequest("Invalid Exemplar object");
                }
                updateExemplarDTO.Id = id;
                ExemplarDTO employeeDTO = await employeeService.UpdateExemplar(updateExemplarDTO);
                if (employeeDTO == null)
                {
                    logger.Error($"Exemplar with id: {id}, hasn't been found in db.");
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

        // DELETE api/<ExemplarController>/5
        [HttpDelete("{id}", Name = "DeleteExemplar")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await employeeService.DeleteExemplar(id);
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
