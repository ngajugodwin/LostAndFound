using LostAndFound_API.Domain.Services;
using LostAndFound_API.Extensions;
using LostAndFound_API.Resources.Auth;
using LostAndFound_API.Resources.Response;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] AccessResource accessResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var result = await _authService.LoginAsync(accessResource.Email.ToLower(), accessResource.Password);

            if (!result.Success)
                return Unauthorized(new ApiResponse
                (
                    ApiResult.STATUS_FAILED,
                    result.Message
                ));

            return Ok(new ApiResponse
            (
                ApiResult.STATUS_SUCCESS,
                "Login Successful",
                result.Resource.User                
            ));
        }
    }
}
