using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

    }
}