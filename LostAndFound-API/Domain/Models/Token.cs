
using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Domain.Models
{
    public class Token
    {
        public int Id { get; set; }

        public string ClientId { get; set; }

        public string Value { get; set; }

        public long UserId { get; set; }

        public virtual User User { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime ExpiryTime { get; set; }

    }
}
