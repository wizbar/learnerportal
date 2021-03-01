using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class BbbeeRating
    {
        public BbbeeRating()
        {
            TrainingProviders = new HashSet<TrainingProvider>();
        }
        [Key]
        public long BeeRatingId { get; set; }
        public string BeeRatingDesc { get; set; }
        public decimal BeeRecognition { get; set; }
        public virtual ICollection<TrainingProvider> TrainingProviders { get; set; }
    }
}
