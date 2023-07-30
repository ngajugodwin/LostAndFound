using AutoMapper;
using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Resources.Item;
using LostAndFound_API.Resources.Response;
using LostAndFound_API.Resources.UserItemBookmark;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Mvc;

namespace LostAndFound_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserBookmarksController : ControllerBase
    {
        private readonly IGenericRepository _genericRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserItemBookmarkRepository _userItemBookmarkRepository;

        public UserBookmarksController(IGenericRepository genericRepository, 
            IMapper mapper, IUnitOfWork unitOfWork, 
            IUserItemBookmarkRepository userItemBookmarkRepository)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userItemBookmarkRepository = userItemBookmarkRepository;
        }

        [HttpGet("{userId}", Name = "GetUserItemsBookmarkAsync")]
        public async Task<IActionResult> GetUserItemsBookmarkAsync(long userId)
        {
            var userItemBookmarks = await _userItemBookmarkRepository.GetUserItemBookmark(userId);

            if (userItemBookmarks.Count() <= 0)
            {
                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, "No data to return"));
            }

            return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, ApiResult.SUCCESS_MESSAGE, userItemBookmarks));
        }

        [HttpGet("validateUserItemBookmark/{userId}/{itemId}")]
        public async Task<IActionResult> ValidateUserItemBookmark(long userId, int itemId)
        {

            var item = await _genericRepository.FindAsync<Item>(x => x.Id == itemId);

            if (item == null)
                return Ok(new ApiResponse(ApiResult.STATUS_FAILED, "Item does not exist"));


            var result = await _userItemBookmarkRepository.UserHasSimilarBookmark(userId, itemId);

            if (result)
            {
                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, "Item already bookmarked for user", new
                {
                    ItemIsBookmark = true
                }));
            }
            else
            {
                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, "Item is not bookmarked for user", new
                {
                    ItemIsBookmark = false
                })); ;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddUserItemBookmark([FromBody] UserItemBookmarkRequest userItemBookmarkRequest)
        {
            var result = await _userItemBookmarkRepository.UserHasSimilarBookmark(userItemBookmarkRequest.UserId, userItemBookmarkRequest.ItemId);

            if (result)
                return Ok(new ApiResponse(ApiResult.STATUS_FAILED, "Item already bookmarked"));

            var userItemBookmark = new UserItemBookmark
            {
                BookmarkByUserId = userItemBookmarkRequest.UserId,
                ItemBookmarkId = userItemBookmarkRequest.ItemId,
            };

            try
            {
                await _genericRepository.AddAsync(userItemBookmark);
                await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, "New item bookmarked successfully",
                    new
                    {
                        ItemId = userItemBookmark.ItemBookmarkId,
                        UserId = userItemBookmark.BookmarkByUserId
                    }));
            }
            catch (Exception ex)
            {

                return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, ex.Message));
            }
        }

        [HttpPost("removeUserItemBookmark")]
        public async Task<IActionResult> RemoveUserItemBookmark([FromBody] UserItemBookmarkRequest userItemBookmarkRequest)
        {
            var result = await _genericRepository
                .FindAsync<UserItemBookmark>(x => x.ItemBookmarkId == userItemBookmarkRequest.ItemId
                && x.BookmarkByUserId == userItemBookmarkRequest.UserId);

            if (result == null)
                return Ok(new ApiResponse(ApiResult.STATUS_FAILED, "Bookmark not found"));

            try
            {
                _userItemBookmarkRepository.DeleteUserItemBookmark(result);
                await _unitOfWork.CompleteAsync();

                return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, "Bookmark removed successfully",
                      new
                      {
                          ItemId = result.ItemBookmarkId,
                          UserId = result.BookmarkByUserId
                      }));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, ex.Message));
            }

            

        }
    }
}
