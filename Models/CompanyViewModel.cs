using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace learner_portal.Models
{
    public class CompanyViewModel
    {
        [NotMapped]
        public  IFormFile Photo { get; set; }
        public Address Address { get; set; }
        
        public Company Company { get; set; }
    }
}