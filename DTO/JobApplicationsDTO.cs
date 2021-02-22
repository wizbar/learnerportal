using System;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class JobApplicationsDTO
    {
        // Job Information
        
        
        [DisplayName("Learner ID")]
        public long LearnerId { get; set; }             
        
        [DisplayName("Job ID")]
        public long? JobId { get; set; }     
        
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        
        [DisplayName("Date Applied")]
        public string DateApplied { get; set; }
        
        [DisplayName("Expiry Date")]
        public string ExpiryDate { get; set; }
        
        [DisplayName("Sector")]
        public string SectorDesc { get; set; }
        
        [DisplayName("Job Type")]
        public string JobTypeDesc { get; set; }
        public string JobApplicationStatus { get; set; }
        
        

    }
}
