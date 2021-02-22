using System;
using System.Collections.Generic;
using learner_portal.Models;

namespace learner_portal.Repository 
{
    public interface IEnterpriseLibrary  : IDisposable
    {
        public List<Institution> GetInstitutionData(string key);
        public List<School> GetSchoolData(string key);
        public void Initialize();

    }
}