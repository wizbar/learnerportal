using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class HomeLanguage
    {
        
        public HomeLanguage()
        {
            Person = new HashSet<Person>();
        }
        [Key]
        public long HomeLanguageId { get; set; }
        public string HomeLanguageCode { get; set; }
        public string HomeLanguageDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Person> Person { get; set; }
    }
}