namespace LostAndFound_API.Helpers
{
    public class AppSettings
    {
        public string Site { get; set; }
        public string Audience { get; set; }
        public string ExpireTime { get; set; }
        public string Secret { get; set; }
        public string ClientId { get; set; }

        public string RefreshToken { get; set; } //to decide weather to leave or remove
    }
}
