using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Domain.Models
{
    public class ItemComment
    {
        public  long Id { get; set; }
        public string Comment { get; set; } = string.Empty;

        public long CommentByUserId { get; set; }
        public virtual User? CommentByUser { get; set; }

        public int ItemId { get; set; }
        public virtual Item? Item { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
