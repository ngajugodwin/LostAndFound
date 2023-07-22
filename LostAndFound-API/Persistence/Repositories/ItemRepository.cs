using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Persistence.Context;
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
                .Include(x => x.ClaimedByUser)
                .Where(x => x.Id == id).FirstOrDefaultAsync(x => x.Id == id);

            return (item != null) ? item : null;
        }

        public async Task<IEnumerable<Item>> ListAsync()
        {
            return await _context.Items
                .Include(x => x.ReportedByUser)
                .Include(x => x.ClaimedByUser)
                .Where(x => x.IsActive == true)
                .ToListAsync();
        }

        public Task<Item> UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}
