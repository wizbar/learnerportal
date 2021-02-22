using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Province
    {
        public Province()
        {
            Address = new HashSet<Address>();
            Cities = new HashSet<City>();
            JobApplications = new HashSet<JobApplications>();
        }
        [Key]
        public long ProvinceId { get; set; }
        [DisplayName("Province Name")]
        public string ProvinceName { get; set; }
        [DisplayName("Province Code")]
        public string ProvinceCode { get; set; }
        [DisplayName("Country")]
        public long? CountryId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<City> Cities { get; set; }
        public virtual ICollection<JobApplications> JobApplications { get; set; }
    }
}