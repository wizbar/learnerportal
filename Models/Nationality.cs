using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Nationality
    {
        public Nationality()
        {
            Person = new HashSet<Person>();
        }
        
        [Key]
        public long NationalityId { get; set; }
        public string NationalityCode { get; set; }
        public string NationalityDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}