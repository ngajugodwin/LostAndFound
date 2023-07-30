namespace LostAndFound_API.Resources.User
{
    public class UserSettingResource
    {
        public long UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public bool IsNotificationActive { get; set; }
    }
}
