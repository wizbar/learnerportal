using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Company
    {
        public Company()
        {
            Address = new HashSet<Address>();
            Documents = new HashSet<Document>();
        }
        [Key]
        public long CompanyId { get; set; }
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Company Registration No")]
        public string CompanyRegistrationNo { get; set; }     
        
        [DisplayName("SDL")]
        public string SDL { get; set; }
        [DisplayName("Date Business Commenced")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateBusinessCommenced { get; set; }
        [DisplayName("Contact Name")]
        public string ContactName { get; set; }
        [DisplayName("Contact Surname")]
        public string ContactSurname { get; set; }
        [DisplayName("Contact Email")]
        public string ContactEmail { get; set; }
        [DisplayName("Contact Mobile")]
        public string ContactMobile { get; set; }
        [DisplayName("Contact Telephone")]
        public string ContactTelephone { get; set; }
        [DisplayName("Photo Path")] 
        
        public string PhotoPath { get; set; }
                
        [DisplayName("Photo Name")]
        
        public string PhotoName { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; }
        
        public virtual ICollection<Address> Address { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }
    }
}