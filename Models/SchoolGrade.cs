using System.Collections.Generic;
using System.ComponentModel;

namespace learner_portal.Models
{
    public class SchoolGrade
    {
                
        public SchoolGrade()
        {
            Learner = new HashSet<Learner>();
        }

        public long SchoolGradeId { get; set; }
        [DisplayName("School Grade Code")]
        public string SchoolGradeCode { get; set; }
        [DisplayName("School Grade Name")]
        public string SchoolGradeName { get; set; }

        public virtual ICollection<Learner> Learner { get; set; }
    }
}