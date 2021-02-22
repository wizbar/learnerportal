using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public  class Learner
    {
        public Learner()
        {
            LearnerCourse = new List<LearnerCourse>()
            {

            };
            JobApplications = new HashSet<JobApplications>();
            Documents = new HashSet<Document>();
        }


        [Key]
        public long LearnerId { get; set; } 
        [DisplayName("Person")]
        public Guid PersonId { get; set; }
        [DisplayName("School")]
        public long SchoolId { get; set; }
        [DisplayName("School Grade")]
        public long SchoolGradeId { get; set; }
        
        [DisplayName("Motivation Text")]
        public string MotivationText { get; set; }
        [DisplayName("Year Completed")]
        public DateTime YearSchoolCompleted { get; set; }
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        [DisplayName("Date Created")]
        public DateTime? DateCreated { get; set; } 
        [DisplayName("Last Updated By")]
        public string LastUpdatedBy { get; set; }
        [DisplayName("Date Updated")]
        public DateTime? DateUpdated { get; set; } 
        public string RecruitedYn { get; set; } 
        public string AppliedYn { get; set; } 
        public Person Person { get; set; }
        public School School { get; set; }
        public SchoolGrade SchoolGrade { get; set; }

        public ICollection<LearnerCourse> LearnerCourse { get; set; }
        
        public virtual ICollection<JobApplications> JobApplications { get; set; }
        
        public virtual ICollection<Document> Documents { get; set; }

    }
}