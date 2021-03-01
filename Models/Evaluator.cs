using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Evaluator
    {
        public Evaluator()
        {
            Assessors = new HashSet<Assessor>();
            Moderators = new HashSet<Moderator>();
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public Guid EvaluatorsId { get; set; }
        public Guid PersonId { get; set; }
        public virtual ICollection<Assessor> Assessors { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }

    }
}
 