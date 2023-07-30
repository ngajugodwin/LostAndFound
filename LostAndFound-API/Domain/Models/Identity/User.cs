using LostAndFound_API.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;

namespace LostAndFound_API.Domain.Models.Identity
{
    public class User : IdentityUser<long>
    {
        public string FullName { get; set; }

        public override string Email { get; set; }

        public override string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }

        public virtual ICollection<Token> Tokens { get; set; }

        public string? UserPhotoUrl { get; set; }
        public string? PublicId { get; set; }

        public virtual ICollection<Item> ItemReportedByUser { get; set; }
        public virtual ICollection<Item> ItemClaimedByUser { get; set; }

        public virtual ICollection<ItemComment> ItemCommentByUser { get; set; }

        public virtual ICollection<UserItemBookmark> UserItemBookmarks { get; set; }

        public virtual UserNotificationSetting UserNotificationSetting { get; set; }

        public User()
        {
            Tokens = new HashSet<Token>();
            UserRoles = new HashSet<UserRole>();
            ItemReportedByUser = new HashSet<Item>();
            ItemClaimedByUser = new HashSet<Item>();
            ItemCommentByUser = new HashSet<ItemComment>();
            UserItemBookmarks = new HashSet<UserItemBookmark>();
            CreatedAt = DateTime.Now;
        }
    }
}
