using System;
using System.ComponentModel;

namespace learner_portal.Models
{
    public class EmailTemplates
    {
        public EmailTemplates()
        { 
           
        }
        
        public System.Guid Id { get; set; }
        [DisplayName("Config Name")]
        public string Name { get; set; }

        public string  Description { get; set; }
        
        public string Subject { get; set; }
        public string EmailBody { get; set; }

        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateUpdated { get; set; }
        [DisplayName("Updated By")]
        public string UpdatedBy { get; set; }
    }
}