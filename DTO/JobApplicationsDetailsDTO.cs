using System.Collections.Generic;

namespace learner_portal.DTO
{
    public class JobApplicationsDetailsDTO : PersonDetailsDTO
    {
        
        public List<JobApplicationsDTO> Applications = new List<JobApplicationsDTO>();
        
    }
}