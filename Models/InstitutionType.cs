using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
  
namespace learner_portal.Models
{
    public class InstitutionType
    {
        
        public InstitutionType()
        {
            Institution = new HashSet<Institution>();
        }
        [Key]
        public long InstitutionTypeId { get; set; }
        [DisplayName("Institution Type Description")]
        public string InstitutionTypeDesc { get; set; }
        [DisplayName("Institution Type Code")]
        public string InstitutionTypeCode { get; set; }

        public virtual ICollection<Institution> Institution { get; set; }
    }
}