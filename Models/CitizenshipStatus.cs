using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class CitizenshipStatus
    {
        public CitizenshipStatus()
        {
            Person = new HashSet<Person>();
        }
        [Key]
        public long CitizenshipStatusId { get; set; }
        public string CitizenshipStatusCode { get; set; }
        public string CitizenshipStatusDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}