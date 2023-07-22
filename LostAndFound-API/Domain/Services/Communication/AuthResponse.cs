using LostAndFound_API.Resources.Auth;

namespace LostAndFound_API.Domain.Services.Communication
{
    public class AuthResponse : BaseResponse<LoginResource>
    {
        /// <summary>
        /// Creates a success response.
        /// </summary>
        /// <param name="user">User response.</param>
        /// <returns>Response.</returns>
        public AuthResponse(LoginResource loginResource) : base(loginResource)
        { }

        /// <summary>
        /// Creates am error response.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <returns>Response.</returns>
        public AuthResponse(string message) : base(message)
        { }
    }
}
