using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Extensions;
using LostAndFound_API.Helpers;
using LostAndFound_API.Resources.Item;
using LostAndFound_API.Resources.Response;
using LostAndFound_API.Services.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LostAndFound_API.Controllers
{
  
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IItemService _itemService;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public ItemsController(IMapper mapper,
            IItemService itemService, 
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _mapper = mapper;
            _itemService = itemService;
            _cloudinaryConfig = cloudinaryConfig;

            Account acct = new Account(_cloudinaryConfig.Value.CloudName, _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);
            _cloudinary = new Cloudinary(acct);
        }

        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemAsync(int itemId)
        {
            var item = await _itemService.GetItemByIdAsync(itemId);

            if (item.Resource == null)
                return BadRequest(new ApiResponse(ApiResult.STATUS_FAILED, "Item not found!"));

            var itemToReturn = _mapper.Map<ItemResource>(item.Resource);

            return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, ApiResult.SUCCESS_MESSAGE, itemToReturn));
        }

        [HttpPost]
        public async Task<IActionResult> CreateItemAsync([FromForm] SaveItemResource saveItemResource)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.GetErrorMessages());

            var itemToSave = _mapper.Map<SaveItemResource, Item>(saveItemResource);

            ProcessPhoto(saveItemResource.File, itemToSave);

            try
            {
                await _itemService.SaveAsync(itemToSave);
                var itemToReturn = _mapper.Map<Item, ItemResource>(itemToSave);
                return Ok(new ApiResponse
                (
                    ApiResult.STATUS_SUCCESS,
                    "New lost item created successfully!",
                    itemToReturn
                ));
            }
            catch (Exception ex)
            {
                return Ok(new ApiResponse
               (
                   ApiResult.STATUS_FAILED,
                   ex.Message
               ));
            }
        }

        [HttpGet]
        public async Task<IActionResult> ListAsync()
        {
            var items = await _itemService.ListAsync();

            var itemToReturn = _mapper.Map<IEnumerable<ItemResource>>(items);

            return Ok(new ApiResponse(ApiResult.STATUS_SUCCESS, ApiResult.SUCCESS_MESSAGE, itemToReturn));
        }

        private Item ProcessPhoto(IFormFile file, Item item)
        {
            var uploadResult = new ImageUploadResult();

           
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face") // transform image to capture relevant areas
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                   
                }
            }

            item.PictureUrl = uploadResult.Url.ToString();
            item.PublicId = uploadResult.PublicId;

            return item;
        }

    }
}
