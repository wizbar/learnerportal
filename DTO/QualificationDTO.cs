using System;
using System.ComponentModel;      

namespace learner_portal.DTO 
{
    public class QualificationDTO
    {

        [DisplayName("Institution Name ")]
        public string InstitutionName { get; set; } 
        [DisplayName("Qualification Name")]
        public string CourseName { get; set; }
        [DisplayName("Year Completed")]
        public DateTime DateOfCompletion { get; set; }
    }
} 