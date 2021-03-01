using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Seta
    {
        public Seta()
        {
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long SetaId { get; set; }
        public string SetaCode { get; set; }
        public string SetaDesc { get; set; }
        public string ActiveYn { get; set; }
        public long EtqeId { get; set; }
        public virtual Etqe Etqe { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}