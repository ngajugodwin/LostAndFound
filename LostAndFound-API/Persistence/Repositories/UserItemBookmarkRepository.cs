using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Persistence.Context;
using LostAndFound_API.Resources.UserItemBookmark;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound_API.Persistence.Repositories
{
    public class UserItemBookmarkRepository : BaseRepository, IUserItemBookmarkRepository
    {
        public UserItemBookmarkRepository(ApplicationDbContext context)
                : base(context)
        {

        }

        public async Task<List<UserItemBookmarkResource>> GetUserItemBookmark(long userId)
        {
            return await _context.UserItemBookmarks
                .Include(x => x.ItemBookmark).ThenInclude(x => x.ReportedByUser)
                .Where(x => x.BookmarkByUserId == userId).Select(x => new UserItemBookmarkResource
                {
                    ItemId = x.ItemBookmark.Id,
                    Name = x.ItemBookmark.Name,
                    Color = x.ItemBookmark.Color,
                    Description = x.ItemBookmark.Description,
                    LostLocationAddress = x.ItemBookmark.LostLocationAddress,
                    RetrievalLocationAddress = x.ItemBookmark.RetrievalLocationAddress,
                    Note = x.ItemBookmark.Note,
                    IsActive = x.ItemBookmark.IsActive,
                    CreatedAt = x.ItemBookmark.CreatedAt,
                    Contact = GetItemContact(x.ItemBookmark),
                    PictureUrl = x.ItemBookmark.PictureUrl,
                    ReportedByUserId = x.ItemBookmark.ReportedByUserId
                }).OrderByDescending(x => x.CreatedAt).ToListAsync();

        }

        public void DeleteUserItemBookmark(UserItemBookmark userItemBookmark)
        {
            _context.UserItemBookmarks.Remove(userItemBookmark);
        }

        public async Task<bool> UserHasSimilarBookmark(long userId, int itemId)
        {
            var result =  await _context.UserItemBookmarks                
                .AnyAsync(x => x.BookmarkByUserId == userId && x.ItemBookmarkId == itemId);

            return result ? true : false;
        }


        private static string GetItemContact(Item item)
        {
            if (item != null)
            {
                switch (item.ModeOfContact)
                {
                    case ModeOfContact.Email:
                        return item.ReportedByUser.Email;
                    case ModeOfContact.Phone:
                        return item.ReportedByUser.PhoneNumber;                       
                    default:
                        return item.ReportedByUser.Email;
                        break;
                }
            }
            return string.Empty;
        }
    }
}
