using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Person
    {
        public Person()
        {
            Address = new HashSet<Address>();
            Email = "admin@me.com";
        }
        
        [Key]
        public Guid Id { get; set; }
        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter FirstName")]
        public string FirstName { get; set; }
        
        [DisplayName("Title")]
        [Required(ErrorMessage = "Please enter Title")]
        public string Title { get; set; }
        public string UserId { get; set; }
        
        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter LastName")]
        public string LastName { get; set; }
        
        [DisplayName("Email")] 
       public string Email { get; set; }
        
        [DisplayName("Phone Number")]
        [Required(ErrorMessage = "Please enter PhoneNumber")]
        public string PhoneNumber { get; set; }
        
        [DisplayName("Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Please enter Date of Birth")]
        public DateTime PersonsDob { get; set; }
        
        [DisplayName("Gender")]
        [Required(ErrorMessage = "Please enter Gender")]
        public long? GenderId { get; set; }
        
        [DisplayName("National Id")]
       [Required(ErrorMessage = "Please enter National ID")]
        public string NationalId { get; set; }
        [DisplayName("Equity")]
        public long? EquityId { get; set; }
        [DisplayName("Nationality")]
        public long? NationalityId { get; set; }
        [DisplayName("Home Language")]
        public long? HomeLanguageId { get; set; }
        [DisplayName("Citizenship Status")]
        [Required]
        public long? CitizenshipStatusId { get; set; }
        [DisplayName("Disability Status")]
        public long? DisabilityStatusId { get; set; }
     
        [DisplayName("Photo Path")] 
        
        public string PhotoPath { get; set; }
                
        [DisplayName("Photo Name")]
        
        public string PhotoName { get; set; }
        
        [DisplayName("CV Path")] 
        
        public string CvPath { get; set; }
                
        [DisplayName("CV Name")]
        public string CvName { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }
        
        public virtual Users User { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual Equity Equity { get; set; }
        public virtual Nationality Nationality { get; set; }
        public virtual HomeLanguage HomeLanguage { get; set; }
        public virtual CitizenshipStatus CitizenshipStatus { get; set; }
        public virtual DisabilityStatus DisabilityStatus { get; set; }
      
        public virtual ICollection<Address> Address { get; set; }


    }
}