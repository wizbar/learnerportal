using System;
using System.ComponentModel;      

namespace learner_portal.DTO 
{
    public class QualificationDTO
    {
        [DisplayName("Id")]
        public long Id { get; set; } 
        [DisplayName("Institution Name ")]
        public string InstitutionName { get; set; } 
        [DisplayName("Qualification Name")]
        public string CourseName { get; set; }
        [DisplayName("Year Completed")]
        public DateTime DateOfCompletion { get; set; }
    }
} 