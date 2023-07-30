using LostAndFound_API.Resources.User;

namespace LostAndFound_API.Domain.Repositories
{
    public interface IUserNotificationSettingRepository
    {
        Task<UserSettingResource?> GetUserNotificationSettings(long userId);
    }
}
