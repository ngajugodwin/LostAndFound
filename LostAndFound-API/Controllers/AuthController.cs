﻿using LostAndFound_API.Domain.Services;
using LostAndFound_API.Extensions;
using LostAndFound_API.Resources.Auth;
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
                return Unauthorized(result.Message);

            return Ok(result.Resource);
        }
    }
}
