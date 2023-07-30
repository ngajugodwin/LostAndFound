namespace LostAndFound_API.Domain.Notification
{
    public class Mail
    {
        public string EmailTo { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
