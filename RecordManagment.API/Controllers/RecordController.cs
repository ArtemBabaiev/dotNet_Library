using Microsoft.AspNetCore.Mvc;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.Service.Interface;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecordManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly ILogger logger;
        private IRecordService employeeService;

        public RecordController(ILogger logger, IRecordService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        // GET: api/<RecordController>
        [HttpGet(Name = "GetAllRecords")]
        public async Task<ActionResult<IEnumerable<RecordDTO>>> Get()
        {
            try
            {
                var result = await employeeService.GetAllRecords();
                logger.Information($"Returned all employees from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<RecordController>/5
        [HttpGet("{id}", Name = "GetRecordById")]
        public async Task<ActionResult<RecordDTO>> Get(long id)
        {
            try
            {
                var result = await employeeService.GetRecordById(id);
                if (result == null)
                {
                    logger.Error($"Record with id: {id}, hasn't been found in db.");
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

        // POST api/<RecordController>
        [HttpPost(Name = "CreateRecord")]
        public async Task<ActionResult> Post([FromBody] RecordDTO newRecordDTO)
        {
            try
            {
                if (newRecordDTO == null)
                {
                    logger.Error("Record object sent from client is null.");
                    return BadRequest("Record object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Record object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdRecordDTO = await employeeService.CreateRecord(newRecordDTO);
                return CreatedAtRoute("GetRecordById", new { id = createdRecordDTO.Id }, createdRecordDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside POST action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<RecordController>/5
        [HttpPut("{id}", Name = "UpdateRecord")]
        public async Task<ActionResult> Put(long id, [FromBody] RecordDTO updateRecordDTO)
        {
            try
            {
                if (updateRecordDTO == null)
                {
                    logger.Error("Record object sent from client is null.");
                    return BadRequest("Record object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Record object sent from client.");
                    return BadRequest("Invalid Record object");
                }
                updateRecordDTO.Id = id;
                RecordDTO employeeDTO = await employeeService.UpdateRecord(updateRecordDTO);
                if (employeeDTO == null)
                {
                    logger.Error($"Record with id: {id}, hasn't been found in db.");
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

        // DELETE api/<RecordController>/5
        [HttpDelete("{id}", Name = "DeleteRecord")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await employeeService.DeleteRecord(id);
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
