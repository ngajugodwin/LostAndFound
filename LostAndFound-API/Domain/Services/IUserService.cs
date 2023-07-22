using LostAndFound_API.Domain.Models.Identity;
using LostAndFound_API.Domain.Services.Communication;

namespace LostAndFound_API.Domain.Services
{
    public interface IUserService
    {
        Task<UserResponse> SaveAsync(User user, string password);
        Task<UserResponse> GetUserByIdAsync(long id);
        Task<IEnumerable<User>> ListAsync();
    }
}
