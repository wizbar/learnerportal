using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace learner_portal.Models
{
    public  class JobApplications
    {
        public JobApplications()
        {
            Documents = new HashSet<Document>();
        }
        
        [Key]
        public Guid? Id { get; set; }
        [DisplayName("Date Applied")]
        public DateTime DateApplied { get; set; }
        [DisplayName("Application status")]
        public string ApplicationStatus { get; set; }
        
        [DisplayName("Learner")]
        public long LearnerId { get; set; }
        
        [DisplayName("Job")]
        public long? JobId { get; set; }
        
        public virtual Learner Learner { get; set; }
        public virtual Job Job { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }
    }
}
