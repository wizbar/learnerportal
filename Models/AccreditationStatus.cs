using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class AccreditationStatuses 
    {
        public AccreditationStatuses()
        {
            Assessors = new HashSet<Assessor>();
            Moderators = new HashSet<Moderator>();
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long AccreditationStatusId { get; set; }
        public string AccreditationStatusDesc { get; set; }
        public long RoleId { get; set; }
        public string AccreditationStatusCode { get; set; }

        public virtual ICollection<Assessor> Assessors { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}
