using System;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class DocumentDetailsDTO
    {
        public Guid Id { get; set; }
        [DisplayName("Document Type")]
        public string DocumentTypeName { get; set; }
        [DisplayName("Comments")]
        public string Comment { get; set; }
        [DisplayName("LearnerId")]
        public string LearnerId { get; set; }
        [DisplayName("Company")]
        public string CompanyName { get; set; }
        
        [DisplayName("File Name")]
        public string FileName { get; set; }
        [DisplayName("File Path")]
        public string FilePath { get; set; }
        [DisplayName("Verified")]
        public string Verified { get; set; }
        [DisplayName("Verification Date")]
        public DateTime? VerificationDate { get; set; }
        [DisplayName("Verified By")]
        public string VerifiedBy { get; set; }
        [DisplayName("Job Application")]
        public Guid? JobApplication { get; set; }
    }
}