using System.ComponentModel.DataAnnotations;

namespace LostAndFound_API.Resources.Item
{
    public class UserCommentResource
    {
        [Required]
        public long UserId { get; set; }

        [Required]
        public int ItemId { get; set; }

        [Required]
        public string Comment { get; set; } = string.Empty;
    }
}
