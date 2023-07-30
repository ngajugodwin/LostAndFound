using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Services.Communication;
using LostAndFound_API.Resources.Item;

namespace LostAndFound_API.Domain.Services
{
    public interface IItemService
    {
        Task<IEnumerable<Item>> ListAsync(ItemQuery itemQuery);

        Task<ItemResponse> GetItemByIdAsync(int itemId);

        Task<ItemResponse> SaveAsync(Item item);

        Task<ItemResponse> UpdateAsync(int itemId, Item item);

        Task<ItemResponse> CloseLostItemAsync(int itemId, long userId, string closingRemarks);

        Task<ItemCommentResponse> AddUserCommentToItem(string comment, long userId, int itemId);

        Task<IEnumerable<ItemComment>> GetItemCommentsAsync(int itemId);
    }
}
