using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class ProgrammeType
    {
        public ProgrammeType()
        {
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long ProgrammeTypeId { get; set; }
        public string ProgrammeTypeDesc { get; set; }
        public string ActiveYn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public string DateUpdated { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}
