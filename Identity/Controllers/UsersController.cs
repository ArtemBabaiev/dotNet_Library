using Identity.Models;
using Identity.Services;
using Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("add-role")]
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

        [HttpPost("authenticate")]
        public async Task<IActionResult> GetTokenAsync(TokenRequestModel model)
        {
            try
            {
                logger.LogInformation("Trying to authenticate user");
                var result = await userService.GetTokenAsync(model);
                SetRefreshTokenInCookie(result.RefreshToken); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something failed during authentication of user: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                logger.LogInformation("Trying to refresh token");
                var refreshToken = Request.Cookies["refreshToken"];
                var response = await userService.RefreshTokenAsync(refreshToken);
                if (!string.IsNullOrEmpty(response.RefreshToken))
                    SetRefreshTokenInCookie(response.RefreshToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError($"Something failed during refresh token: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
            var response = userService.RevokeToken(token);
            if (!response)
                return NotFound(new { message = "Token not found" });
            return Ok(new { message = "Token revoked" });
        }


        [Authorize]
        [HttpPost("{id}/refresh-tokens")]
        public IActionResult GetRefreshTokens(string id)
        {
            var user = userService.GetById(id);
            return Ok(user.RefreshTokens);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetUser(string id)
        {
            return Ok(userService.GetById(id));
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            return Ok(userService.GetAll());
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }


}
