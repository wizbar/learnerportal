using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace learner_portal.Models
{
    public class Course
    {
        [Key]
        public long CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDesc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}