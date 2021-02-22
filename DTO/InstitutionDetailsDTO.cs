using System.ComponentModel;

namespace learner_portal.DTO
{
    public class InstitutionDetailsDTO
    {
        public long Id { get; set; }
        [DisplayName("Institution Code")]
        public string InstitutionCode { get; set; }
        
        [DisplayName("Institution Name")]
        public string InstitutionName { get; set; }
        [DisplayName("Institution Type")]
        public string InstitutionTypeDesc { get; set; }
    }
}