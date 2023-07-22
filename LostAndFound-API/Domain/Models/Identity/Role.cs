using Microsoft.AspNetCore.Identity;

namespace LostAndFound_API.Domain.Models.Identity
{
    public class Role : IdentityRole<long>
    {
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
