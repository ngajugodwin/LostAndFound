using LostAndFound_API.Domain.Services.Communication;

namespace LostAndFound_API.Domain.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
    }
}
