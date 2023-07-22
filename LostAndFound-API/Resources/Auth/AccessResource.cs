using System.ComponentModel.DataAnnotations;

namespace LostAndFound_API.Resources.Auth
{
    public class AccessResource
    {
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
