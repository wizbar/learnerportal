using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class PersonDetailsDTO
    {
        // Biography Information 
        [DisplayName("Learner")]
        public long LearnerId { get; set; }
        // Biography Information   
        [DisplayName("First Name")]
        public string FirstName { get; set; } 
        
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        
        [DisplayName("Email")]
        public string Email { get; set; }
         
        [DisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
        
        [DisplayName("Date of Birth")]
        public string PersonsDob { get; set; }
        
        
        [DisplayName("Age")]
        public int Age { get; set; }
        
        [DisplayName("Gender")]  
        public string GenderName { get; set; }
        
        [DisplayName("National ID")]
        public string NationalID { get; set; }
        
        [DisplayName("Equity")]
        public string EquityName { get; set; }
        
        [DisplayName("Nationality")]
        public string Nationality { get; set; }
        
        [DisplayName("Home Language")]
        public string HomeLanguage { get; set; }
        
        [DisplayName("Citizenship Status")]
        public string CitizenshipStatus { get; set; }
        
        [DisplayName("Disability Status")]
        public string DisabilityStatus { get; set; }

        [DisplayName("Photo Path")]
        public string PhotoPath { get; set; }
        
        [DisplayName("Photo Name")]
        public string PhotoName { get; set; }

        [DisplayName("House No")]
        public string HouseNo { get; set; }
        [DisplayName("Street Name")]
        public string StreetName { get; set; }
        [DisplayName("Suburb")] 
        public string SuburbName{ get; set; }
        [DisplayName("City")]
        public string CityName { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; }
        [DisplayName("Address Type")]
        public string AddressType { get; set; }
        
        [DisplayName("School")]
        public string SchoolName { get; set; }
        
        [DisplayName("Grade")]
        public string SchoolGradeName { get; set; }
        
        [DisplayName("Year Completed")]
        public string YearSchoolCompleted { get; set; }

        public List<QualificationDTO> Qualifications = new List<QualificationDTO>();
        
        public List<DocumentDetailsDTO> Documents = new List<DocumentDetailsDTO>();
    }
}