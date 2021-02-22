using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class School
    {
        
        public School()
        {
            Learner = new HashSet<Learner>();
        }
        [Key]
        public long SchoolId { get; set; } 
        [DisplayName("School Code")]
        public string SchoolCode { get; set; }
        [DisplayName("EMIS No")]
        public string EmisNo { get; set; }
        [DisplayName("School Name")]
        public string SchoolName { get; set; }
        
        public virtual ICollection<Learner> Learner { get; set; }
    }
}