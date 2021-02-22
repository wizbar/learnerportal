using System.Collections.Generic;
using System.ComponentModel;

namespace learner_portal.DTO
{
    public class LearnerDetailsDto : PersonDetailsDTO
    { 
        // Learner Information  
 
        public LearnerDetailsDto()
        {
            JobApplicationsDto = new HashSet<JobApplicationsDTO>();

        }
         
        [DisplayName("Company")]
        public string CompanyName { get; set; }
   
        [DisplayName("Motivation Text")]
        public string MotivationText { get; set; }
        public string RecruitedYn { get; set; }
        public string AppliedYn { get; set; }

        public ICollection<JobApplicationsDTO> JobApplicationsDto { get; set; }


    }
}