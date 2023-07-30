using AutoMapper;
using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Resources.Response;
using LostAndFound_API.Resources.User;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserNotificationSettingsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository _genericRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserNotificationSettingRepository _userNotificationSettingRepository;

        public UserNotificationSettingsController(IMapper mapper, IGenericRepository genericRepository,            
            IUnitOfWork unitOfWork,
            IUserNotificationSettingRepository userNotificationSettingRepository)
        {
            _mapper = mapper;
            _genericRepository = genericRepository;
            _unitOfWork = unitOfWork;
            _userNotificationSettingRepository = userNotificationSettingRepository;
        }

        [HttpPost("activiateOrDisableNotification")]
        public async Task<IActionResult> ActivateOrDisableNotification([FromBody] UserSettingRequest userSettingRequest)
        {
            var userNotification = await _genericRepository.FindAsync<UserNotificationSetting>(x => x.UserId == userSettingRequest.UserId);

            if (userNotification != null)
            {
                userNotification.Status = userSettingRequest.Status;
                await _unitOfWork.CompleteAsync();

                var msg = (userNotification.Status) ? "Notification is activated" : "Notification is disabled";

                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, msg, new {UserId = userNotification.UserId, Status = userNotification.Status}));
            }

            return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, "No settings defined for this user"));        
        }

        [HttpGet("getUserSettings/{userId}")]
        public async Task<IActionResult> GetUserSettings(long userId)
        {
            var userSettings = await _userNotificationSettingRepository.GetUserNotificationSettings(userId);

            if (userSettings != null)
                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, ApiResult.SUCCESS_MESSAGE, userSettings));

            return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, "No settings defined for this user"));

        }
    }
}
