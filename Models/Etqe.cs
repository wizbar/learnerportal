using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Etqe
    {
        public Etqe()
        {
            Assessors = new HashSet<Assessor>();
            Moderators = new HashSet<Moderator>();
            Setas = new HashSet<Seta>();
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long EtqeId { get; set; }
        public string EtqeCode { get; set; }
        public string EtqeName { get; set; }
        public virtual ICollection<Assessor> Assessors { get; set; }
        public virtual ICollection<Moderator> Moderators { get; set; }
        public virtual ICollection<Seta> Setas { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}
