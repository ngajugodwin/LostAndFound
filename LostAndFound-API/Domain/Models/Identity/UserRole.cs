using Microsoft.AspNetCore.Identity;

namespace LostAndFound_API.Domain.Models.Identity
{
    public class UserRole : IdentityUserRole<long>
    {
        public User User { get; set; }
        public Role Role { get; set; }
    }
}
