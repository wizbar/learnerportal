using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class RegisterUserDto
    {
        [Required (ErrorMessage = "* Please enter username")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
        
        [Required (ErrorMessage = "* Please enter email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required (ErrorMessage = "* Please enter password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }    
        
        [Required (ErrorMessage = "* Please enter confirm password")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }        
        
        [Required]
      
        [Display(Name = "Role")]
        public string Role { get; set; } 
        
        [StringLength(15, ErrorMessage = "Please provide a valid phone number", MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Active(Y/N)")] 
        public string ActiveYn { get; set; }

    }
}