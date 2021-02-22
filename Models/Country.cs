using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Country
    {
        public Country()
        {
            Address = new HashSet<Address>();
            Provinces = new HashSet<Province>();
        }
        [Key]
        public long CountryId { get; set; }
        [DisplayName("Country Name")]
        public string CountryName { get; set; }
        [DisplayName("Country Code")]
        public string CountryCode { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Province> Provinces { get; set; }
    }
}