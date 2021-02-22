using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Institution
    {
        [Key]
        public long InstitutionId { get; set; }
        [DisplayName("Institution Code")]
        public string InstitutionCode { get; set; } 
        
        [DisplayName("Institution Name")]
        public string InstitutionName { get; set; } 
        [DisplayName("Institution Type")]
        public long? InstitutionTypeId { get; set; }
        
        public virtual InstitutionType InstitutionType { get; set; }
    }
}