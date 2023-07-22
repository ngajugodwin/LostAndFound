namespace LostAndFound_API.Resources.User
{
    public class UserResource
    {
        public long Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string[] UserRoles { get; set; }
    }
}
