using AutoMapper;
using LostAndFound_API.Domain.Models.Identity;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Extensions;
using LostAndFound_API.Resources.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public AccountsController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewAccountAsync([FromForm] SaveUserResource saveUserResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var userToSave = _mapper.Map<SaveUserResource, User>(saveUserResource);

            var result = await _userService.SaveAsync(userToSave, saveUserResource.Password);

            if (!result.Success)
            {
                var errorData = new
                {
                    Message = result.Message,
                    Error = result.Errors
                };
                return BadRequest(new
                {
                    // Data = errorData
                    Code = "FAILED",
                    Message = errorData
                });
            }

            var userToReturn = _mapper.Map<UserResource>(result.Resource);

            return Ok(new
            {
                Code = "SUCCESS",
                Message = "Account created successfully",
                Data = userToReturn
            });
        }
    }
}
