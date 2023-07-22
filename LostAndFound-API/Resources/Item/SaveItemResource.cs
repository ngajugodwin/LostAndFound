using LostAndFound_API.Domain.Models;

namespace LostAndFound_API.Resources.Item
{
    public class SaveItemResource
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LostLocationAddress { get; set; } = string.Empty;
        public string RetrievalLocationAddress { get; set; } = string.Empty;
        public string? Note { get; set; }
        public IFormFile File { get; set; }
        public ModeOfContact ModeOfContact { get; set; }
        public long ReportedByUserId { get; set; }

        public SaveItemResource()
        {

        }
    }
}
