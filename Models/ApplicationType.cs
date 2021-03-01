﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class ApplicationType
    {
        public ApplicationType()
        {
            Assessors = new HashSet<Assessor>();
            Moderators = new HashSet<Moderator>();
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long ApplicationTypesId { get; set; }
        public string ApplicationTypesDesc { get; set; }
        
        public virtual ICollection<Assessor> Assessors { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}
 