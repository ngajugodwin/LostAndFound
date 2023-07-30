using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Domain.Models
{
    public class UserItemBookmark
    {
        public long BookmarkByUserId { get; set; }
        public virtual User? BookmarkByUser { get; set; }

        public int ItemBookmarkId { get; set; }
        public virtual Item? ItemBookmark { get; set; }
    }
}
