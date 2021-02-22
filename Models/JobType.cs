using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable disable

namespace learner_portal.Models
{
    public class JobType
    {
        public JobType()
        {
            Jobs = new HashSet<Job>();
        }
        public long JobTypeId { get; set; }
        [DisplayName("Job Type Code")]
        public string JobTypeCode { get; set; }
        [DisplayName("Job Type Description")]
        public string JobTypeDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
