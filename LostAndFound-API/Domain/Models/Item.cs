using LostAndFound_API.Domain.Models.Identity;
using System.ComponentModel;

namespace LostAndFound_API.Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public string LostLocationAddress { get; set; } = string.Empty;
        public string RetrievalLocationAddress { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string? PictureUrl { get; set; }
        public string? PublicId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public ModeOfContact  ModeOfContact { get; set; }
        public long ReportedByUserId { get; set; }
        public virtual User ReportedByUser { get; set; }
        public long? ClaimedByUserId { get; set; }
        public virtual User? ClaimedByUser { get; set; }
        public string? ClosingRemarks { get; set; }

        public virtual ICollection<ItemComment> ItemComments { get; set; }
        public virtual ICollection<UserItemBookmark> UserItemBookmarks { get; set; }

        public Item()
        {
            ItemComments = new HashSet<ItemComment>();
            UserItemBookmarks = new HashSet<UserItemBookmark>();
        }

    }

    public enum ModeOfContact 
    {
        [Description("Email")]
        Email = 1,

        [Description("Phone")]
        Phone =2,
    }
}
