using System.ComponentModel.DataAnnotations;

namespace LostAndFound_API.Resources.User
{
    public class SaveUserResource
    {

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
