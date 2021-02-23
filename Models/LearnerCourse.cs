using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.Build.Tasks;

namespace learner_portal.Models
{
    public class LearnerCourse 
    {

        [Key] public long LearnerCourseId { get; set; }
        [DisplayName("Learner")]   
        public long LearnerId { get; set; }
        
        [DisplayName("Course Name")] 
        [Required(ErrorMessage = "Please capture Qualification")  ]
        public string CourseName { get; set; }
        [DisplayName("Institution Name")] 
        [Required(ErrorMessage = "Please capture Institution")  ]
        public string InstitutionName { get; set; }
        [Required(ErrorMessage = "Please capture date of completion")  ]
        public DateTime DateOfCompletion { get; set; }
        [DisplayName("Created By")] 
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")] 
        public DateTime? DateCreated { get; set; }
        [DisplayName("Last Updated By")] 
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")] 
        public DateTime? DateUpdated { get; set; }
       public virtual Learner Learner { get; set; }
    }
}