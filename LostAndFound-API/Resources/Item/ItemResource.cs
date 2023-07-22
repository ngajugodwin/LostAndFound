using LostAndFound_API.Domain.Models;

namespace LostAndFound_API.Resources.Item
{
    public class ItemResource
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LostLocationAddress { get; set; } = string.Empty;
        public string RetrievalLocationAddress { get; set; } = string.Empty;
        public string? Note { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string Contact { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
