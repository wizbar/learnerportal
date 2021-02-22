using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Gender
    {
        public Gender()
        {
            Person = new HashSet<Person>(); 
        }
        
        [Key]
        public long GenderId { get; set; }
        public string GenderCode { get; set; }
        public string GenderDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}