using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.DTO
{
    public class JobDetailsDTO
    {
        [DisplayName("Job Id")]
        public long Id { get; set; }
        
        [DisplayName("Job Code")]
        public string JobCode { get; set; }
        [DisplayName("Job Title")]
        public string JobTitle { get; set; }
        [DisplayName("Job Description")]
        public string JobDesc { get; set; }
        
        [DisplayName("Date Listed")]
        public DateTime? ListedDate { get; set; }
        
        [DisplayName("Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")] 
        public DateTime? ExpiryDate { get; set; }
        [DisplayName("Sector")]
        public string SectorDesc { get; set; }
        [DisplayName("Job Type")]
        public string JobTypeDesc { get; set; }
        [DisplayName("Job Organising Framework for Occupations (OFO)")]
        public string OfoTitle { get; set; }
        [DisplayName("Province")]
        public string ProvinceName { get; set; }
        [DisplayName("Company")]
        public string CompanyName { get; set; }
    }
}