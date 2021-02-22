using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Suburb
    {
        public Suburb()
        {
            Address = new HashSet<Address>();
        }
        [Key]
        public long SuburbId { get; set; }
        public string SuburbName { get; set; }
        public string SuburbCode { get; set; }
        public long? CityId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        
    }
}