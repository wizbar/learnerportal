using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class City
    {
        public City()
        {
            Address = new HashSet<Address>();
            Suburbs = new HashSet<Suburb>();
        }
        [Key]
        public long CityId { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public long? ProvinceId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual Province Province { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<Suburb> Suburbs { get; set; }
    }
}