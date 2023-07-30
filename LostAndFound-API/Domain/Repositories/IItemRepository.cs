using LostAndFound_API.Domain.Models;
using LostAndFound_API.Resources.Item;

namespace LostAndFound_API.Domain.Repositories
{
    public interface IItemRepository
    {
        Task<Item> GetItem(int id);
        Task<Item> UpdateItem(Item item);
        void DeleteItem(Item item);

        Task CreateItem(Item item);

        Task<IEnumerable<Item>> ListAsync(ItemQuery itemQuery);

        Task<List<ItemComment>> GetItemCommentsAsync(int itemId);
    }
}
