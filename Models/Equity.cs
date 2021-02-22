using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Equity
    {
        public Equity()
        {
            Person = new HashSet<Person>();
        }
        
        [Key]
        public long EquityId { get; set; }
        public string EquityCode { get; set; }
        public string EquityDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}