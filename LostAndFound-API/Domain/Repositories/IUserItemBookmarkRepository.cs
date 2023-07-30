using LostAndFound_API.Domain.Models;
using LostAndFound_API.Resources.UserItemBookmark;

namespace LostAndFound_API.Domain.Repositories
{
    public interface IUserItemBookmarkRepository
    {
        Task<List<UserItemBookmarkResource>> GetUserItemBookmark(long userId);
        void DeleteUserItemBookmark(UserItemBookmark userItemBookmark);
        Task<bool> UserHasSimilarBookmark(long userId, int itemId);
    }
}
