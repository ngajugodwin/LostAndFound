using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Persistence.Context;
using LostAndFound_API.Resources.Item;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound_API.Persistence.Repositories
{
    public class ItemRepository : BaseRepository, IItemRepository
    {
        public ItemRepository(ApplicationDbContext context)
                : base(context)
        {

        }

        public async Task CreateItem(Item item)
        {
            await _context.Items.AddAsync(item);
        }

        public void DeleteItem(Item item)
        {
            _context.Items.Remove(item);
        }

        public async Task<Item> GetItem(int id)
        {
            var item = await _context.Items
                .Include(x => x.ReportedByUser)
                .Include(x => x.ClaimedByUser).ThenInclude(x => x.ItemCommentByUser)
                .Include(x => x.ItemComments).ThenInclude(x => x.CommentByUser)
                .Where(x => x.Id == id).FirstOrDefaultAsync(x => x.Id == id);

            return (item != null) ? item : null;
        }

        public async Task<List<ItemComment>> GetItemCommentsAsync(int itemId)
        {
            return await _context.ItemComments.Include(x => x.CommentByUser)
                                              .Where(i => i.ItemId == itemId)
                                              .OrderByDescending(x => x.CreatedAt)
                                              .ToListAsync();
        }

        public async Task<IEnumerable<Item>> ListAsync(ItemQuery itemQuery)
        {
            var result =  _context.Items
                .Include(x => x.ReportedByUser)
                .Include(x => x.ClaimedByUser).ThenInclude(x => x.ItemCommentByUser)
                .Include(x => x.ItemComments).ThenInclude(x => x.CommentByUser)
                .Where(x => x.IsActive == true).AsQueryable();

            switch (itemQuery.SortBy)
            {
                case SortBy.MostRecentLostItem:
                    result = result.OrderByDescending(x => x.CreatedAt);
                    break;
                case SortBy.OldLostItem:
                    result = result.OrderByDescending(x => x.CreatedAt).Reverse();
                    break;
                default:
                    result = result.OrderBy(x => x.Name);
                    break;
            }

            if (!string.IsNullOrWhiteSpace(itemQuery.Search))
            {
                result = result.Where(x => x.Name.Contains(itemQuery.Search));
            }

            return await result.ToListAsync();

        }

        //public async Task<IEnumerable<Item>> ListAsync()
        //{
        //    return await _context.Items
        //        .Include(x => x.ReportedByUser)
        //        .Include(x => x.ClaimedByUser)
        //        .Where(x => x.IsActive == true)
        //        .ToListAsync();
        //}

        public Task<Item> UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
