namespace LostAndFound_API.Resources.UserItemBookmark
{
    public class UserItemBookmarkResource
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LostLocationAddress { get; set; } = string.Empty;
        public string RetrievalLocationAddress { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Contact { get; set; } = string.Empty;

        public long ReportedByUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
