using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService userService;
        ILogger<UsersController> logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            this.userService = userService;
            this.logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterModel model)
        {
            try
            {
                logger.LogInformation("Trying to register user");
                var result = await userService.RegisterAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something failed in registering user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            try
            {
                logger.LogInformation("Trying to authenticate user");
                var result = await userService.GetTokenAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something failed during authentication of user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRoleAsync(AddRoleModel model)
        {
            try
            {
                logger.LogInformation("Trying to add role to user");
                var result = await userService.AddRoleAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something failed during role assigment: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

        }
    }
}
