using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class DisabilityStatus
    {
        public DisabilityStatus()
        {
            Person = new HashSet<Person>();
        }
        [Key]
        public long DisabilityStatusId { get; set; }
        public string DisabilityStatusCode { get; set; }
        public string DisabilityStatusDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}