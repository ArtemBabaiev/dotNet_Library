using Microsoft.AspNetCore.Mvc;
using RecordManagment.BLL.DTO;
using RecordManagment.BLL.Service.Interface;
using ILogger = Serilog.ILogger;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecordManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteratureController : ControllerBase
    {
        private readonly ILogger logger;
        private ILiteratureService employeeService;

        public LiteratureController(ILogger logger, ILiteratureService employeeService)
        {
            this.logger = logger;
            this.employeeService = employeeService;
        }


        // GET: api/<LiteratureController>
        [HttpGet(Name = "GetAllLiteratures")]
        public async Task<ActionResult<IEnumerable<LiteratureDTO>>> Get()
        {
            try
            {
                var result = await employeeService.GetAllLiterature();
                logger.Information($"Returned all employees from database.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Transaction Failed! Something went wrong inside Get() action: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET api/<LiteratureController>/5
        [HttpGet("{id}", Name = "GetLiteratureById")]
        public async Task<ActionResult<LiteratureDTO>> Get(long id)
        {
            try
            {
                var result = await employeeService.GetLiteratureById(id);
                if (result == null)
                {
                    logger.Error($"Literature with id: {id}, hasn't been found in db.");
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

        // POST api/<LiteratureController>
        [HttpPost(Name = "CreateLiterature")]
        public async Task<ActionResult> Post([FromBody] LiteratureDTO newLiteratureDTO)
        {
            try
            {
                if (newLiteratureDTO == null)
                {
                    logger.Error("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Literature object sent from client.");
                    return BadRequest("Invalid model object");
                }
                var createdLiteratureDTO = await employeeService.CreateLiterature(newLiteratureDTO);
                return CreatedAtRoute("GetLiteratureById", new { id = createdLiteratureDTO.Id }, createdLiteratureDTO);
            }
            catch (Exception ex)
            {
                logger.Error(ex, $"Something went wrong inside POST action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/<LiteratureController>/5
        [HttpPut("{id}", Name = "UpdateLiterature")]
        public async Task<ActionResult> Put(long id, [FromBody] LiteratureDTO updateLiteratureDTO)
        {
            try
            {
                if (updateLiteratureDTO == null)
                {
                    logger.Error("Literature object sent from client is null.");
                    return BadRequest("Literature object is null");
                }
                if (!ModelState.IsValid)
                {
                    logger.Error("Invalid Literature object sent from client.");
                    return BadRequest("Invalid Literature object");
                }
                updateLiteratureDTO.Id = id;
                LiteratureDTO employeeDTO = await employeeService.UpdateLiterature(updateLiteratureDTO);
                if (employeeDTO == null)
                {
                    logger.Error($"Literature with id: {id}, hasn't been found in db.");
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

        // DELETE api/<LiteratureController>/5
        [HttpDelete("{id}", Name = "DeleteLiterature")]
        public async Task<ActionResult> Delete(long id)
        {
            try
            {
                await employeeService.DeleteLiterature(id);
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
