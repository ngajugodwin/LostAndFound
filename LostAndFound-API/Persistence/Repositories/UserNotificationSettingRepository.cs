using LostAndFound_API.Domain.Models;
using LostAndFound_API.Domain.Repositories;
using LostAndFound_API.Persistence.Context;
using LostAndFound_API.Resources.User;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound_API.Persistence.Repositories
{
    public class UserNotificationSettingRepository : BaseRepository, IUserNotificationSettingRepository
    {
        public UserNotificationSettingRepository(ApplicationDbContext context)
               : base(context)
        {

        }

        public async Task<UserSettingResource?> GetUserNotificationSettings(long userId)
        {
            var result = await _context.UserNotificationSettings
                .Include(x => x.User)
                .Where(x => x.UserId == userId).FirstOrDefaultAsync(x => x.UserId == userId);

            if (result != null)
            {
                var userSettings = new UserSettingResource
                {
                    UserId = result.UserId,
                    FullName = result.User.FullName,
                    IsNotificationActive = result.Status
                };

                return userSettings;
            }

            return null;
        }

       
    }
}
