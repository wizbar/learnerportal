using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace learner_portal.Models
{
    public partial class Document
    {
        [Key]
        public Guid Id { get; set; }
        [DisplayName("File Path")]
        public string FilePath { get; set; }
        [DisplayName("File Name")]
        public string FileName { get; set; }
        [DisplayName("Document Type")]
        public Guid DocumentTypeId { get; set; }
        [DisplayName("Comments")]
        public string Comments { get; set; }
        [DisplayName("Learner")]
        public long? LearnerId { get; set; }
        [DisplayName("Company")]
        public long? CompanyId { get; set; }  
        [DisplayName("Verified")]
        public string Verified { get; set; }
        [DisplayName("Verification Date")]
        public DateTime? VerificationDate { get; set; }
        [DisplayName("Verified By")]
        public string VerifiedBy { get; set; }
        [DisplayName("Job Application")]
        public Guid JobApplicationId { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated{ get; set; } 

        [NotMapped] 
        public IFormFile MyFiles { get; set; }
        
        
        public virtual DocumentType DocumentType { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual Company Company { get; set; }
        public virtual JobApplications JobApplications { get; set; }
    }
}
