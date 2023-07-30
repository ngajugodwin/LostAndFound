using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Notification;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Domain.Services.Communication;
using LostAndFound_API.Resources.Item;

namespace LostAndFound_API.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;
        private readonly IGenericRepository _genericRepository;
        private readonly INotificationService<Mail> _notificationService;

        public ItemService(IUnitOfWork unitOfWork, IItemRepository itemRepository, IGenericRepository genericRepository, INotificationService<Mail> notificationService)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
            _genericRepository = genericRepository;
            _notificationService = notificationService;
        }
        public async Task<ItemResponse> GetItemByIdAsync(int itemId)
        {
            var itemFromRepo = await _itemRepository.GetItem(itemId);

            if (itemFromRepo == null)
                return new ItemResponse("Item not found");

            return new ItemResponse(itemFromRepo);
        }

        public async Task<IEnumerable<ItemComment>> GetItemCommentsAsync(int itemId)
        {
            return await _itemRepository.GetItemCommentsAsync(itemId);
        }

        public async Task<ItemCommentResponse> AddUserCommentToItem(string comment, long userId, int itemId)
        {
            try
            {
                var newComment = new ItemComment
                {
                    Comment = comment,
                    CommentByUserId = userId,
                    ItemId = itemId,
                    CreatedAt = DateTime.Now
                };

                await _genericRepository.AddAsync<ItemComment>(newComment);
                await NotifyReporter(itemId);
                await _unitOfWork.CompleteAsync();                

                return new ItemCommentResponse(newComment);
            }
            catch (Exception ex)
            {
                return new ItemCommentResponse($"An error occured when add the comment: {ex.Message}");
            }
        }       

        public async Task<IEnumerable<Item>> ListAsync(ItemQuery itemQuery)
        {
            return await _itemRepository.ListAsync(itemQuery);
        }

        public async Task<ItemResponse> SaveAsync(Item item)
        {
            try
            {
                item.CreatedAt = DateTime.Now;
                item.IsActive = true;
                await _itemRepository.CreateItem(item);
                await _unitOfWork.CompleteAsync();

                return new ItemResponse(item);
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
                
        public async Task<ItemResponse> CloseLostItemAsync(int itemId, long userId, string closingRemarks)
        {
            var itemFromRepo = await _itemRepository.GetItem(itemId);

            if (itemFromRepo == null)
                return new ItemResponse("Item not found");

            itemFromRepo.IsActive = false;
            // itemFromRepo.ClaimedByUserId = userId;
            itemFromRepo.ClosingRemarks = closingRemarks;

            try
            {
                await _unitOfWork.CompleteAsync();
                return new ItemResponse(itemFromRepo);
            }
            catch (Exception ex)
            {
                return new ItemResponse($"An error occured when closing the item: {ex.Message}");
            }

        }

        public async Task<ItemResponse> UpdateAsync(int itemId, Item item)
        {
            var itemFromRepo = await _itemRepository.GetItem(itemId);

            if (itemFromRepo == null)
                return new ItemResponse("Item not found");

            itemFromRepo.Note = item.Note;
            itemFromRepo.Name = item.Name;
            itemFromRepo.Description = item.Description;
            itemFromRepo.Color = item.Color;
            itemFromRepo.RetrievalLocationAddress= item.RetrievalLocationAddress;
            itemFromRepo.LostLocationAddress = item.LostLocationAddress;
            itemFromRepo.ModeOfContact = item.ModeOfContact;

            try
            {
                await _unitOfWork.CompleteAsync();
                return new ItemResponse(itemFromRepo);
            }
            catch (Exception ex)
            {
                return new ItemResponse($"An error occured when updating the item: {ex.Message}");
            }
        }

        private async Task NotifyReporter(int itemId)
        {
            var itemFromRepo = await _itemRepository.GetItem(itemId);

            var userSetting = await _genericRepository.FindAsync<UserNotificationSetting>(x => x.UserId == itemFromRepo.ReportedByUserId);
                        
            if (userSetting != null && userSetting.Status)
            {
                var mailBody = PrepareNotificationMailBody(itemFromRepo);

                await _notificationService.SendNotification(new Mail { Body = mailBody, EmailTo = itemFromRepo.ReportedByUser.Email, Subject = "LostAndFound - New Comment Added" });
            }         
            
        }

        private string PrepareNotificationMailBody(Item item)
        {
            var mailBody = string.Empty;

            mailBody = $"<p>Dear {item.ReportedByUser.FullName},</p>" + "</b>"
                        + "<p style=\"text-align: justify;\">Please be informed that a comment has been added to your post.&nbsp;</p>"

                        + "<p style=\"text-align: justify;\">Thank you for using our system to report the lost item.</p>"
                        + "<br>"
                        + "<br>"
                        + "<p style=\"text-align: justify;\">for: LostAndFound - SOULS Team Software.</p>"
                        + "<p style=\"text-align: justify;\">Tel: 123456</p>"
                        + "<p style=\"text-align: justify;\">Email: equipsystems@gmail.com</p>";

            return mailBody;
        }
    }
}
