using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

#nullable disable

namespace learner_portal.Models
{
    public class Job 
    {
        public Job()
        {
            JobApplications = new HashSet<JobApplications>();
        }
        
        public long JobId { get; set; }
        [DisplayName("Job Code")]
        public string JobCode { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        [DisplayName("Job Description")]
        public string JobDesc { get; set; }
        
        [DisplayName("Date Listed")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ListedDate { get; set; }
        
        [DisplayName("Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")] 
        public DateTime ExpiryDate { get; set; }
        [DisplayName("Sector")]
        public long? JobSectorId { get; set; }
        [DisplayName("Sector")]
        public long? SectorId { get; set; }
        [DisplayName("Job Type")]
        public long? JobTypeId { get; set; }
        [DisplayName("Job Organising Framework for Occupations (OFO)")]
        public long? OfoId { get; set; }
        [DisplayName("Photo Path")]
        public string JobPhotoPath { get; set; }
        
        [DisplayName("Province")]
        public long? ProvinceId { get; set; }

        [DisplayName("Photo Name")]
        public string JobPhotoName { get; set; }
        
        [DisplayName("Company")]
         public long? CompanyId { get; set; }
        
        [NotMapped]
        [DisplayName("Job Image")]
        public IFormFile JobPhoto { get; set; }
        public virtual JobSector JobSector { get; set; }
        public virtual Company Company { get; set; }
        public virtual Sector Sector { get; set; }
        public virtual JobType JobType { get; set; }
        public virtual Ofo Ofo { get; set; }
        public virtual Province Province { get; set; }
        
        public virtual ICollection<JobApplications> JobApplications { get; set; }
    }
}
