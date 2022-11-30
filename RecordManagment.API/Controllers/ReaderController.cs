using Microsoft.AspNetCore.Mvc;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.Service.Interface;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecordManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReaderController : ControllerBase
    {
        private readonly ILogger logger;
        private IReaderService employeeService;

        public ReaderController(ILogger logger, IReaderService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        // GET: api/<ReaderController>
        [HttpGet(Name = "GetAllReaders")]
        public async Task<ActionResult<IEnumerable<ReaderDTO>>> Get()
        {
            try
            {
                var result = await employeeService.GetAllReaders();
                logger.Information($"Returned all employees from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<ReaderController>/5
        [HttpGet("{id}", Name = "GetReaderById")]
        public async Task<ActionResult<ReaderDTO>> Get(long id)
        {
            try
            {
                var result = await employeeService.GetReaderById(id);
                if (result == null)
                {
                    logger.Error($"Reader with id: {id}, hasn't been found in db.");
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

        // POST api/<ReaderController>
        [HttpPost(Name = "CreateReader")]
        public async Task<ActionResult> Post([FromBody] ReaderDTO newReaderDTO)
        {
            try
            {
                if (newReaderDTO == null)
                {
                    logger.Error("Reader object sent from client is null.");
                    return BadRequest("Reader object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Reader object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdReaderDTO = await employeeService.CreateReader(newReaderDTO);
                return CreatedAtRoute("GetReaderById", new { id = createdReaderDTO.Id }, createdReaderDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside POST action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<ReaderController>/5
        [HttpPut("{id}", Name = "UpdateReader")]
        public async Task<ActionResult> Put(long id, [FromBody] ReaderDTO updateReaderDTO)
        {
            try
            {
                if (updateReaderDTO == null)
                {
                    logger.Error("Reader object sent from client is null.");
                    return BadRequest("Reader object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Reader object sent from client.");
                    return BadRequest("Invalid Reader object");
                }
                updateReaderDTO.Id = id;
                ReaderDTO employeeDTO = await employeeService.UpdateReader(updateReaderDTO);
                if (employeeDTO == null)
                {
                    logger.Error($"Reader with id: {id}, hasn't been found in db.");
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

        // DELETE api/<ReaderController>/5
        [HttpDelete("{id}", Name = "DeleteReader")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await employeeService.DeleteReader(id);
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
