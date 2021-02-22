using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Please enter Username")]
        public string Username { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter password")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public string Role { get; set; }
    }
} 