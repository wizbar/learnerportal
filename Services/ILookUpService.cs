using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using learner_portal.DTO;
using learner_portal.Models;

namespace learner_portal.Services
{
    public interface ILookUpService
    {
        // Start DataTables Methods
        public Task<List<Lookup>> GetProvinces();
        public Task<Users> GetCurrentLoggedInUser(string username);
        public Task<List<Province>> GetAllProvince();
        public Task<List<Lookup>> GetCities();
        public Task<List<Lookup>> GetCountries();
        public Task<List<Country>> GetAllCountries(); 
        public Task<List<City>> GetAllCities();
        public Task<List<Lookup>> GetSuburbs();
        public Task<List<Lookup>> GetCourses();
        public Task<List<Suburb>> GetAllSuburbs();
        public Task<List<Person>> GetAllPerson();
        public Task<List<Institution>> GetAllInstitution();
        public Task<List<Lookup>> GetInstitutions();
        public Task<List<InstitutionType>> GetAllInstitutionType();
        public Task<List<Job>> GetAllJob();
        public Task<List<JobSector>> GetAllJobSector();
        
        public Task<List<Lookup>> GetJobSectors();
        public Task<List<Lookup>> GetJobTypes();
        public Task<List<JobType>> GetAllJobType();
       // public List<Users> GetAllUsers();
        public Task<List<Ofo>> GetAllOfo();
        public Task<List<OfoMinor>>GetAllOfoMinor();
        public Task<List<Financialyear>> GetAllFinancialyear();
        public Task<List<Sector>> GetAllSector(); 
        public Task<List<OfoUnit>> GetAllOfoUnit();
        public Task<List<Learner>> GetAllLearners();
        public Task<List<School>> GetAllSchool();
        public Task<List<Lookup>> GetSchools(); 
        public Task<List<Lookup>> GetSchoolGrades(); 
        public Task<List<SchoolGrade>> GetAllSchoolGrade(); 
        public Task<List<Address>> GetAllAddress();
        public Task<List<Lookup>> GetAddressTypes();
        // End DataTables Methods
        
        
        // Methods For Person
        public Task<PersonDetailsDTO> GetPersonDetailsByEmail(string email);
        public Task<List<PersonDetailsDTO>> GetPersonDetails();    
        public Task<PersonDetailsDTO> GetPersonByNationalId(string id);
        public Task<Learner> GetPersonByNationalIdForEditDelete(string id); 
        public Task<Users> GetUserByUsrname(string name);
        public Task<JobApplicationsDetailsDTO> GetJobApplicantPersonByNationalId(string id);
        public Task<PersonDetailsDTO> GetPersonDetailsByUsername(string username);
            
        // Methods For Learner 
        public Task<LearnerDetailsDto> GetLearnerDetailsByIdEmail(string email); 
        public Task<LearnerDetailsDto> GetLearnerDetailsById(long id);
        public Task<List<CompanyDetailsDTO>> GetCompanyDetails();
        public Task<CompanyDetailsDTO> GetCompanyDetailsById(long CompanyRegistrationNo);
        public Task<CompanyViewModel> GetCompanyDetailsByIdForEditDelete(long id); 
         
        // Methods For OFO
        public Task<List<OfoDTO>> GetOFODetails();
        public Task<OfoDTO> GetOFODetailsById(long id);
        public Task<Ofo> GetOFODetailsByIdForEditDelete(long id);

        // Methods For OFO Unit
        public Task<List<OfoUnitDTO>> GetOfoUnitDetails();
        public Task<OfoUnitDTO> GetOFOUnitDetailsById(long id); 
        public Task<OfoUnit> GetOFOUnitDetailsByIdForEditDelete(long id);
        
        
        public Task<List<ProvinceDetailsDTO>> GetProvincesByCountryId(long id);
        public Task<List<CityDetailsDTO>> GetCitiesByProvincesId(long id);
        public Task<List<SuburbsDetailsDTO>> GetSuburbsByCityId(long id);
        
        // Methods For OFO Minor
        public Task<List<OfoMinorDTO>> GetOfoMinorDetails(); 
        public Task<OfoMinorDTO> GetOfoMinorById(long id);
        public Task<OfoMinor> GetOfoMinorByIdForEditDelete(long id);
        
        // Methods For School 
        public Task<List<SchoolDTO>> GetSchoolDetails();
        public Task<SchoolDTO> GetSchoolDetailsById(long id); 
        public Task<School> GetSchoolByIdForEditDelete(long id);
        
        public Task<List<JobApplicationsDetailsDTO>> GetJobApplicationsDetails();

        public Task<DocumentDetailsDTO> GetDocumentById(Guid id);

        //Methods For Institution
        public Task<List<InstitutionDetailsDTO>> GetInstitutionDetails();
        public Task<InstitutionDetailsDTO> GetInstitutionDetailsById(long id);
        public Task<Institution> GetInstitutionByIdForEditDelete(long id);
        
        // Methods For Job
        public Task<List<JobDetailsDTO>> GetJobDetails();
        public Task<JobDetailsDTO> GetJobDetailsById(long id);
        public Task<Job> GetJobDetailsByIdForEditDelete(long id);

        #region Address

         
        // Methods For Country
        public Task<List<CountriesDetailsDTO>> GetCountriesDetails();
        public Task<CountriesDetailsDTO> GetCountriesDetailsById(long id);
        public Task<Country> GetCountriesDetailsByIdForEditDelete(long id);
        
        // Methods For Province
        public Task<List<ProvinceDetailsDTO>> GetProvincesDetails();
        public Task<ProvinceDetailsDTO> GetAllProvincesById(long id);
        public Task<Province> GetProvinceDetailsByIdForEditDelete(long id);

        // Methods For City
        public Task<List<CityDetailsDTO>> GetCitiesDetails();
        public Task<CityDetailsDTO> GetAllCitiesById(long id);
        public Task<City> GetCitiesDetailsByIdForEditDelete(long id);
        
        // Methods For Suburb
        public Task<List<SuburbsDetailsDTO>> GetSuburbsDetails();
        public Task<SuburbsDetailsDTO> GetAllSuburbsById(long id);
        public Task<Suburb> GetSuburbsDetailsByIdForEditDelete(long id);
        
        // Methods For Address Type
        public Task<List<AddressTypeDetailsDTO>> GetAddressTypeDetails();
        public Task<AddressTypeDetailsDTO> GetAllAddressTypeById(long id);
        public Task<AddressType> GetAddressTypeDetailsByIdForEditDelete(long id);

        #endregion
       

        #region Documents

        public Task<List<DocumentTypesDetailsDTO>> GetDocumentTypesDetails();
        public Task<DocumentTypesDetailsDTO> GetAllDocumentTypesById(Guid id); 
        public Task<DocumentType> GetDocumentTypesDetailsByIdForEditDelete(Guid id);
        public Task<List<DocumentTypesDetailsDTO>> GetDocumentTypesDetailsByRole(string role);

     
        // Methods For Documents
        public Task<List<DocumentDetailsDTO>> GetDocumentsDetails();
        /*public Task<DocumentDetailsDTO> GetAllDocumentsById(Guid id); */
        public Task<Document> GetDocumentsDetailsByIdForEditDelete(Guid id);

        #endregion

        
        //Get Country Provinces 
        /*public List<Province> GetAllProvinces(long id);*/

    } 
}