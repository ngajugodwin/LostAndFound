using AutoMapper;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Resources.Response;
using LostAndFound_API.Resources.User;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("getUserProfile")]
        public async Task<IActionResult> GetUserProfile(long userId)
        {
            var result = await _userService.GetUserByIdAsync(userId);

            if (!result.Success)
                return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, result.Message));

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(new ApiResponse
            (
                ApiResult.STATUS_SUCCESS,
                "User profile fetched successfully",
                userToReturn
            ));
        }
    }
}
