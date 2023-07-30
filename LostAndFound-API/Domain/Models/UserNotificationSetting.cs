using LostAndFound_API.Domain.Models.Identity;

namespace LostAndFound_API.Domain.Models
{
    public class UserNotificationSetting
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public virtual User User { get; set; }
        public bool Status { get; set; }

    }
}
