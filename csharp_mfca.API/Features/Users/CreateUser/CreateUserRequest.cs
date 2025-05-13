using System.ComponentModel.DataAnnotations;

namespace csharp_mfca.API.Features.Users.CreateUser
{
    public class CreateUserRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
