using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Domain.Services;
using LostAndFound_API.Domain.Services.Communication;

namespace LostAndFound_API.Services
{
    public class ItemService : IItemService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IItemRepository _itemRepository;

        public ItemService(IUnitOfWork unitOfWork, IItemRepository itemRepository)
        {
            _unitOfWork = unitOfWork;
            _itemRepository = itemRepository;
        }
        public async Task<ItemResponse> GetItemByIdAsync(int itemId)
        {
            var itemFromRepo = await _itemRepository.GetItem(itemId);

            if (itemFromRepo == null)
                return new ItemResponse("Item not found");

            return new ItemResponse(itemFromRepo);
        }

        public async Task<IEnumerable<Item>> ListAsync()
        {
            return await _itemRepository.ListAsync();
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
            catch (Exception)
            {

                throw;
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
    }
}
