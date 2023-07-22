using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Services.Communication;

namespace LostAndFound_API.Domain.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> ListAsync();

        Task<ItemResponse> GetItemByIdAsync(int itemId);

        Task<ItemResponse> SaveAsync(Item item);

        Task<ItemResponse> UpdateAsync(int itemId, Item item);
    }
}
