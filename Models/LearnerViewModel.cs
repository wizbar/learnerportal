using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace learner_portal.Models
{
    public class LearnerViewModel
    {

        [NotMapped]
        public  IFormFile Photo { get; set; }
        [NotMapped]
        public  IFormFile Cv { get; set; } 
        public Person Person { get; set; }
        public Address Address { get; set; }
        public Learner Learner { get; set; }

    }
}