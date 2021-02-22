using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class UserInfoDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool RememberMe { get; set; }

        [Required] 
        public string Role { get; set; }

        
    }
}