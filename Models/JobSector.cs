using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace learner_portal.Models
{
    public class JobSector
    {
        public JobSector()
        {
            Jobs = new HashSet<Job>();
        }
        [Key]
        public long JobSectorId { get; set; }
        [DisplayName("Job Sector Code")]
        public string JobSectorCode { get; set; }
        [DisplayName("Job Sector Description")]
        public string JobSectorDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
        public virtual ICollection<Job> Jobs { get; set; }
    }
}
