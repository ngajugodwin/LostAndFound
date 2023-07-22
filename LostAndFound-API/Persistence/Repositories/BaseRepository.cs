using LostAndFound_API.Persistence.Context;

namespace LostAndFound_API.Persistence.Repositories
{
    public class BaseRepository
    {
        protected readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
