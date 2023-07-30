namespace LostAndFound_API.Resources.Item
{
    public class ItemCommentResource
    {
        public string Comment { get; set; } = string.Empty;
        public string CommentedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
