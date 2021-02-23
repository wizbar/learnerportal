using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using learner_portal.DTO;
using learner_portal.Helpers;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Internal;
using Rotativa.AspNetCore;

namespace learner_portal.Services
{
    public class LookUpService : ILookUpService, IDisposable
    {
        private readonly LearnerContext _context; 
      
        private bool _disposed;
        private readonly UserManager<Users> _userManager;

        public LookUpService(LearnerContext context,UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;

        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                     _context.Dispose();
                }
                this._disposed = true;
            }
        }

        #region Look Up Data for dropdowns
        
        
        public async Task<Users> GetCurrentLoggedInUser(string username )
        {
            var user = await _userManager.Users.Include(p => p.Person)
                .FirstOrDefaultAsync(u => u.UserName.Equals(username));
            return user;
        }         
        
        public async Task<List<Lookup>> GetInstitutions()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Institution --"
                }
            };
            
            var result = await _context.Institution.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.InstitutionId,
                    name = a.InstitutionName
                });

            return list;
        }  
        
        public async Task<Users> GetUserByUsrname(string name)
        {

            return  await _userManager.FindByNameAsync(name);
        }
               
        public async Task<List<Institution>> GetAllInstitution()
        {
            return  await _context.Institution.ToListAsync();
   
        }
        
        public async Task<List<Lookup>> GetInstitutionTypes()
        {
            
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Institution Type --"
                }
            };
            
            var result = await _context.InstitutionType.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.InstitutionTypeId,
                    name = a.InstitutionTypeDesc
                });

            return list;

        }   
        
        public async Task<List<InstitutionType>> GetAllInstitutionType()
        {
            return await _context.InstitutionType.ToListAsync();


        }


        public async Task<List<Job>> GetAllJob()
        {
            return await _context.Jobs.Include(t => t.JobType).Include(s => s.Sector).ToListAsync();
        }
        
        
        public async Task<List<Lookup>> GetJobSectors()
        {

            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Sector --"
                }
            };
            
            var result = await _context.JobSector.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.JobSectorId,
                    name = a.JobSectorDesc
                });

            return list;
        }
       public async Task<List<JobSector>> GetAllJobSector()
        {

            return await _context.JobSector.ToListAsync();
        }

        public async Task<List<Lookup>> GetJobTypes()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Job Type --"
                }
            };
            
            var result = await _context.JobType.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.JobTypeId,
                    name = a.JobTypeDesc
                });

            return list;
        }        
        
        public async Task<List<JobType>> GetAllJobType()
        {

            return await _context.JobType.ToListAsync();

        }
       
        public async Task<List<Lookup>> GetSchools()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select School --"
                }
            };
            
            var result = await _context.School.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.SchoolId,
                    name = a.SchoolName
                });

            return list;
            
        }        
        
        public async Task<List<School>> GetAllSchool()
        {
            return await _context.School.ToListAsync();
        }

        public async Task<List<Lookup>> GetSchoolGrades()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select School Grade --"
                }
            };
            
            var result = await _context.SchoolGrade.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.SchoolGradeId,
                    name = a.SchoolGradeName
                });

            return list;
        }
    
           public async Task<List<SchoolGrade>> GetAllSchoolGrade()
        {
  
            
            return await _context.SchoolGrade.ToListAsync();
            

        }
    
        
        
        public async Task<List<Ofo>> GetAllOfo()
        {

            return await _context.Ofo.ToListAsync();
        }

        public async Task<List<Financialyear>> GetAllFinancialyear()
        {
            return  await _context.Financialyear.ToListAsync();
        }

        public async Task<List<Sector>> GetAllSector()
        {

            return await _context.Sector.ToListAsync();
        }

        public async Task<List<OfoUnit>> GetAllOfoUnit()
        {
            return await _context.OfoUnit.ToListAsync();
        } 
        
        public async Task<List<OfoMinor>> GetAllOfoMinor()
        {
            return await _context.OfoMinor.ToListAsync();
        }
               
        public async Task<List<Learner>> GetAllLearners()
        {
            
            return await _context.Learner  
                .Include(a => a.LearnerCourse)
                .Include(a => a.School)
                .Include(a => a.SchoolGrade)
                .Include(a => a.Person)
                .Include(a => a.Person.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Person.Address).ThenInclude(a => a.City)
                .Include(a => a.Person.Address).ThenInclude(a => a.Province)
                .Include(a => a.Person.Address).ThenInclude(a => a.Country)
                .Include(a => a.Person.Address).ThenInclude(a => a.AddressType) 
                .Include(a => a.Person.Equity) 
                .Include(a => a.Person.Gender)
                .Include(a => a.Person.Nationality)
                .Include(a => a.Person.CitizenshipStatus)
                .Include(a => a.Person.DisabilityStatus)
                .Include(a => a.Person.HomeLanguage)  
                .Include(a => a.LearnerCourse)
                .Include(a => a.JobApplications).ThenInclude(a => a.Job).ThenInclude(a => a.Sector)
                .Include(a => a.JobApplications).ThenInclude(a => a.Job).ThenInclude(a => a.Province)
                .Include(a => a.JobApplications).ThenInclude(a => a.Job).ThenInclude(a => a.JobType).ToListAsync();
        }   

        public async Task<List<Address>> GetAllAddress()
        {
            return await _context.Address.ToListAsync();
        }

        public async Task<List<Lookup>> GetAddressTypes()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Address Type --"
                }
            };
            
            var result = await _context.AddressType.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.AddressTypeId,
                    name = a.AddressTypeName
                });

            return list;
        }


        // Method Implementation for Address Cascading
        public async Task<List<Province>> GetAllProvince()
        {
            return await _context.Province.ToListAsync();
        }

        public async Task<List<Country>> GetAllCountries()
        {
            return await _context.Country.ToListAsync();
        }

        public async Task<List<City>> GetAllCities()
        {
            return await _context.City.ToListAsync();
        }


        public async Task<List<Suburb>> GetAllSuburbs()
        {
            return await _context.Suburb.ToListAsync();  
  
        }   
        
        public async Task<List<Lookup>> GetProvinces()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Province --"
                }
            };
            
            var result = await _context.Province.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.ProvinceId,
                    name = a.ProvinceName
                });

            return list;
        }       
        
        public async Task<List<Lookup>> GetCountries()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Country --"
                }
            };
            
            var result = await _context.Country.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.CountryId,
                    name = a.CountryName
                });

            return list;
        }        
        
        public async Task<List<Lookup>> GetCourses()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Course --"
                }
            };
            
            var result = await _context.Course.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.CourseId,
                    name = a.CourseTitle
                });

            return list;
        }

        public async Task<List<Lookup>> GetCities()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select City --"
                }
            };
            
            var result = await _context.City.ToListAsync();
            
            list.AddRange(from a in result
                select new Lookup
                {
                    id = a.CityId,
                    name = a.CityName
                });

            return list; 
        }


        public async Task<List<Lookup>> GetSuburbs()
        {
            var list = new List<Lookup>
            {
                new Lookup
                {
                    id = 0,
                    name = "-- Select Suburb --"
                }
            };
            
            var result = await _context.Suburb.ToListAsync();
              
            list.AddRange(from a in result   
                select new Lookup 
                {
                    id = a.SuburbId,
                    name = a.SuburbName
                });

            return list;
        }
        
        #endregion  
   
        #region Get All Full Details or Specific record

       
        // Method Implementation for Company
        public async Task<LearnerDetailsDto> GetLearnerDetailsById(long id)
        {
            return await _context.Learner.Include(p => p.Person).ThenInclude(a => a.Address)
                .ThenInclude(a => a.Country)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.City)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.Province)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.AddressType)
                .Include(p => p.Person).ThenInclude(a => a.Equity)
                .Include(p => p.Person).ThenInclude(a => a.Gender)
                .Include(p => p.Person).ThenInclude(a => a.Nationality)
                .Include(p => p.Person).ThenInclude(a => a.CitizenshipStatus)
                .Include(p => p.Person).ThenInclude(a => a.DisabilityStatus)
                .Include(p => p.Person).ThenInclude(a => a.HomeLanguage)
                .Include(s => s.School).Include(s => s.SchoolGrade)
                .Include(s => s.JobApplications).ThenInclude(s => s.Job).ThenInclude(s => s.Province)
                .Where(p => p.LearnerId == id).Select(person => new LearnerDetailsDto
                {
                    LearnerId = person.LearnerId,
                    FirstName = person.Person.FirstName,
                    LastName = person.Person.LastName,
                    Email = person.Person.Email,
                    PhoneNumber = person.Person.PhoneNumber,
                    PersonsDob = person.Person.PersonsDob.ToString(Const.DATE_FORMAT),
                    Age = Utils.CalculateAge(person.Person.PersonsDob),
                    NationalID = person.Person.NationalId,
                    EquityName = person.Person.Equity.EquityDesc,
                    DisabilityStatus = person.Person.DisabilityStatus.DisabilityStatusDesc,
                    HomeLanguage = person.Person.HomeLanguage.HomeLanguageDesc,
                    CitizenshipStatus = person.Person.CitizenshipStatus.CitizenshipStatusDesc,
                    Nationality = person.Person.Nationality.NationalityDesc,
                    GenderName = person.Person.Gender.GenderDesc,
                    HouseNo = person.Person.Address.ToList()[0].HouseNo,
                    StreetName = person.Person.Address.ToList()[0].StreetName,
                    PostalCode = person.Person.Address.ToList()[0].PostalCode,
                    CountryName = person.Person.Address.ToList()[0].Country.CountryName,
                    CityName = person.Person.Address.ToList()[0].City.CityName,
                    SuburbName = person.Person.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = person.Person.Address.ToList()[0].Province.ProvinceName,
                    AddressType = person.Person.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = person.Person.PhotoName,
                    PhotoPath = person.Person.PhotoPath,
                    SchoolName = person.School.SchoolName,
                    SchoolGradeName = person.SchoolGrade.SchoolGradeName,
                    YearSchoolCompleted = person.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                    Qualifications = (List<QualificationDTO>) person.LearnerCourse.Select(a => new QualificationDTO()
                    {
                        CourseName = a.CourseName,
                        InstitutionName = a.InstitutionName,
                        DateOfCompletion = a.DateOfCompletion
                    }),
                    JobApplicationsDto = person.JobApplications.Select(a => new JobApplicationsDTO()
                    {
                        DateApplied = a.DateApplied.ToString(Const.DATE_FORMAT),
                        ExpiryDate = a.Job.ExpiryDate.ToString(Const.DATE_FORMAT),
                        JobTitle = a.Job.JobTitle,
                        SectorDesc = a.Job.Sector.SectorDesc,
                        JobTypeDesc = a.Job.JobType.JobTypeDesc,
                        JobApplicationStatus = a.ApplicationStatus

                    }).ToList()
                }).FirstOrDefaultAsync()
                ;
        }


        public async Task<List<CompanyDetailsDTO>>   GetCompanyDetails()    
        {    
            return await  _context.Company
                .Include(a => a.Address).ThenInclude(a =>a.Country)
                .Include(a => a.Address).ThenInclude(a => a.City)
                .Include(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Address).ThenInclude(a => a.Province)
                .Include(a => a.Address).ThenInclude(a => a.AddressType).Select(c => new CompanyDetailsDTO()
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    CompanyRegistrationNo = c.CompanyRegistrationNo,
                    DateBusinessCommenced = c.DateBusinessCommenced,
                    ContactName = c.ContactName,
                    ContactSurname = c.ContactSurname,
                    ContactEmail = c.ContactEmail,  
                    ContactMobile = c.ContactMobile,
                    ContactTelephone = c.ContactTelephone,
                    HouseNo = c.Address.ToList()[0].HouseNo,
                    StreetName = c.Address.ToList()[0].StreetName ,
                    PostalCode = c.Address.ToList()[0].PostalCode,
                    CountryName = c.Address.ToList()[0].Country.CountryName,
                    CityName = c.Address.ToList()[0].City.CityName,
                    SuburbName = c.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = c.Address.ToList()[0].Province.ProvinceName,
                    AddressType = c.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = c.PhotoName,
                    PhotoPath = c.PhotoPath
                }).ToListAsync();

        }  
        
        public async Task<CompanyDetailsDTO> GetCompanyDetailsById(long id)
        {   
            return await _context.Company.AsQueryable()
                .Include(a => a.Address).ThenInclude(a =>a.Country)
                .Include(a => a.Address).ThenInclude(a => a.City)
                .Include(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Address).ThenInclude(a => a.Province)
                .Include(a => a.Address).ThenInclude(a => a.AddressType).Where(c => c.CompanyId == id ).Select(c => new CompanyDetailsDTO()
                {
                    CompanyId = c.CompanyId,
                    CompanyName = c.CompanyName,
                    CompanyRegistrationNo = c.CompanyRegistrationNo,
                    DateBusinessCommenced = c.DateBusinessCommenced,
                    ContactName = c.ContactName,
                    ContactSurname = c.ContactSurname,
                    ContactEmail = c.ContactEmail,
                    ContactMobile = c.ContactMobile,
                    ContactTelephone = c.ContactTelephone,
                    HouseNo = c.Address.ToList()[0].HouseNo,
                    StreetName = c.Address.ToList()[0].StreetName , 
                    PostalCode = c.Address.ToList()[0].PostalCode,
                    CountryName = c.Address.ToList()[0].Country.CountryName,
                    CityName = c.Address.ToList()[0].City.CityName,
                    SuburbName = c.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = c.Address.ToList()[0].Province.ProvinceName,
                    AddressType = c.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = c.PhotoName,
                    PhotoPath = c.PhotoPath
                }).FirstOrDefaultAsync();
        }       
        
        
        public async Task<PersonDetailsDTO> GetPersonDetailsByUsername(string username)
        {
             var user = await _userManager.FindByNameAsync(username);

             return await GetPersonDetailsByEmail(user.Email);
        }
        
        public async Task<CompanyViewModel> GetCompanyDetailsByIdForEditDelete(long id)
        {
            
            var company =  await _context.Company.Include(a => a.Address).ThenInclude(a =>a.Country)
                .Include(a => a.Address).ThenInclude(a => a.City)
                .Include(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Address).ThenInclude(a => a.Province)
                .Include(a => a.Address).ThenInclude(a => a.AddressType).Where(c => c.CompanyId == id).FirstOrDefaultAsync();
            CompanyViewModel cmp = new CompanyViewModel();
 
            cmp.Company = company;
            cmp.Address = new Address()
            {
               AddressId    = company.Address.FirstOrDefault().AddressId,
               HouseNo = company.Address.FirstOrDefault().HouseNo,
               StreetName = company.Address.FirstOrDefault().StreetName,
               CityId = company.Address.FirstOrDefault().CityId,
               SuburbId = company.Address.FirstOrDefault().SuburbId,
               ProvinceId = company.Address.FirstOrDefault().ProvinceId,
               CountryId = company.Address.FirstOrDefault().CountryId,
              PostalCode = company.Address.FirstOrDefault().PostalCode,
              DateUpdated = DateTime.Now
             
 
            } ;
            return cmp;
        }
        // Method Implementation for Person

       public async  Task<List<Person>> GetAllPerson()
        {
            return  await _context.Person.ToListAsync();
        }

       public async Task<List<PersonDetailsDTO>> GetPersonDetails()   
        {  
            
            var personDetails =
                await _context.Person.Include(a => a.Address)
                    .ThenInclude(a => a.Country)
                    .Include(a => a.Address)
                    .ThenInclude(a => a.City)
                    .Include(a => a.Address)
                    .ThenInclude(a => a.Suburb)
                    .Include(a => a.Address)
                    .ThenInclude(a => a.Province)
                    .Include(a => a.Address)
                    .ThenInclude(a => a.AddressType)
                    .Include(a => a.Equity)
                    .Include(a => a.Gender)
                    .Include(a => a.Nationality)
                    .Include(a => a.CitizenshipStatus)
                    .Include(a => a.DisabilityStatus)
                    .Include(a => a.HomeLanguage)
                    .Select(p => new PersonDetailsDTO()
                    {  
                        FirstName = p.FirstName, LastName = p.LastName, Email = p.Email, PhoneNumber = p.PhoneNumber,
                        PersonsDob = p.PersonsDob.ToString(Const.DATE_FORMAT),
                        Age = Utils.CalculateAge(p.PersonsDob),
                        NationalID = p.NationalId,
                        EquityName = p.Equity.EquityDesc,
                        DisabilityStatus = p.DisabilityStatus.DisabilityStatusDesc,
                        HomeLanguage = p.HomeLanguage.HomeLanguageDesc,
                        CitizenshipStatus = p.CitizenshipStatus.CitizenshipStatusDesc,
                        Nationality = p.Nationality.NationalityDesc,
                        GenderName = p.Gender.GenderDesc,
                        HouseNo = p.Address.ToList()[0].HouseNo,
                        StreetName = p.Address.ToList()[0].StreetName,
                        PostalCode = p.Address.ToList()[0].PostalCode,
                        CountryName = p.Address.ToList()[0].Country.CountryName,
                        CityName = p.Address.ToList()[0].City.CityName,
                        SuburbName = p.Address.ToList()[0].Suburb.SuburbName,
                        ProvinceName = p.Address.ToList()[0].Province.ProvinceName,
                        AddressType = p.Address.ToList()[0].AddressType.AddressTypeName,
                        PhotoName = p.PhotoName,
                        PhotoPath = p.PhotoPath,
                        
                    }).ToListAsync(); 
 
            return  personDetails; 
        }

        public async Task<PersonDetailsDTO> GetPersonDetailsByEmail(string email)
        {
            
            return await _context.Person
                .Include(a => a.Address).ThenInclude(a => a.Country)
                .Include(a => a.Address).ThenInclude(a => a.City)
                .Include(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Address).ThenInclude(a => a.Province)
                .Include(a => a.Address).ThenInclude(a => a.AddressType)
                .Include(a => a.Equity)
                .Include(a => a.Gender)
                .Include(a => a.Nationality)
                .Include(a => a.CitizenshipStatus)
                .Include(a => a.DisabilityStatus)
                .Include(a => a.HomeLanguage)
                .Include(a => a.HomeLanguage)
                .Where(p => p.Email.Equals(email)).Select(p =>  new PersonDetailsDTO()
                {
               
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.Email,
                    PhoneNumber = p.PhoneNumber,
                    PersonsDob = p.PersonsDob.ToString(Const.DATE_FORMAT),
                    Age = Utils.CalculateAge(p.PersonsDob),
                    NationalID = p.NationalId,
                    EquityName = p.Equity.EquityDesc,
                    DisabilityStatus = p.DisabilityStatus.DisabilityStatusDesc,
                    HomeLanguage = p.HomeLanguage.HomeLanguageDesc,
                    CitizenshipStatus = p.CitizenshipStatus.CitizenshipStatusDesc,
                    Nationality = p.Nationality.NationalityDesc,
                    GenderName = p.Gender.GenderDesc,
                    HouseNo = p.Address.ToList()[0].HouseNo,
                    StreetName = p.Address.ToList()[0].StreetName,
                    PostalCode = p.Address.ToList()[0].PostalCode,
                    CountryName = p.Address.ToList()[0].Country.CountryName,
                    CityName = p.Address.ToList()[0].City.CityName,
                    SuburbName = p.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = p.Address.ToList()[0].Province.ProvinceName,
                    AddressType = p.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = p.PhotoName,
                    PhotoPath = p.PhotoPath

                }).FirstOrDefaultAsync();

            }

        public async Task<PersonDetailsDTO> GetPersonByNationalId(string id)
        {
            return await _context.Learner.Include(p => p.Person).ThenInclude(a => a.Address)
                .ThenInclude(a => a.Country)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.City)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.Suburb)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.Province)
                .Include(p => p.Person).ThenInclude(a => a.Address).ThenInclude(a => a.AddressType)
                .Include(p => p.Person).ThenInclude(a => a.Equity)
                .Include(p => p.Person).ThenInclude(a => a.Gender)
                .Include(p => p.Person).ThenInclude(a => a.Nationality)
                .Include(p => p.Person).ThenInclude(a => a.CitizenshipStatus)
                .Include(p => p.Person).ThenInclude(a => a.DisabilityStatus)
                .Include(p => p.Person).ThenInclude(a => a.HomeLanguage)
                .Include(s => s.School).Include(s => s.SchoolGrade)
                .Include(s => s.Documents).ThenInclude(s => s.DocumentType)
                .Where(p => p.Person.NationalId.Equals(id)).Select(person => new PersonDetailsDTO
                {
                    FirstName = person.Person.FirstName,
                    LastName = person.Person.LastName,
                    Email = person.Person.Email,
                    PhoneNumber = person.Person.PhoneNumber,
                    PersonsDob = person.Person.PersonsDob.ToString(Const.DATE_FORMAT),
                    Age = Utils.CalculateAge(person.Person.PersonsDob),
                    NationalID = person.Person.NationalId,
                    EquityName = person.Person.Equity.EquityDesc,
                    DisabilityStatus = person.Person.DisabilityStatus.DisabilityStatusDesc,
                    HomeLanguage = person.Person.HomeLanguage.HomeLanguageDesc,
                    CitizenshipStatus = person.Person.CitizenshipStatus.CitizenshipStatusDesc,
                    Nationality = person.Person.Nationality.NationalityDesc,
                    GenderName = person.Person.Gender.GenderDesc,
                    HouseNo = person.Person.Address.ToList()[0].HouseNo,
                    StreetName = person.Person.Address.ToList()[0].StreetName,
                    PostalCode = person.Person.Address.ToList()[0].PostalCode,
                    CountryName = person.Person.Address.ToList()[0].Country.CountryName,
                    CityName = person.Person.Address.ToList()[0].City.CityName,
                    SuburbName = person.Person.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = person.Person.Address.ToList()[0].Province.ProvinceName,
                    AddressType = person.Person.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = person.Person.PhotoName,
                    PhotoPath = person.Person.PhotoPath,
                    SchoolName = person.School.SchoolName,
                    SchoolGradeName = person.SchoolGrade.SchoolGradeName,
                    YearSchoolCompleted = person.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                    Qualifications = person.LearnerCourse.Select(a => new QualificationDTO()
                    {
                        Id = a.LearnerCourseId,
                        CourseName = a.CourseName,
                        InstitutionName = a.InstitutionName,
                        DateOfCompletion = a.DateOfCompletion
                    }).ToList(),
                    Documents = person.Documents.Select(a => new DocumentDetailsDTO()
                    {
                        Comment = a.Comments,
                        FileName = a.FileName,
                        LearnerId = a.Learner.Person.FirstName + " " +  a.Learner.Person.FirstName,
                        Verified = a.Verified,
                        Id = a.Id,
                        FilePath = a.FilePath,
                        DocumentTypeName = a.DocumentType.TypeName
                      //  CompanyName = a.Company?.CompanyName
                    }).ToList(),

                    
                }
                    
                ).FirstOrDefaultAsync(); 

        }

        public Task<School> GetSchoolByIdForEditDelete(string id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<JobApplicationsDetailsDTO>> GetJobApplicationsDetails()
        {

           var listOfJobApplications = await  _context.Learner.Include(a => a.JobApplications)
                        .Include(a => a.Person).ThenInclude(g => g.Gender)
                        .Include(a => a.Person).ThenInclude(g => g.Equity)
                        /*.ThenInclude(a => a.Person).ThenInclude(g => g.DisabilityStatus)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.Nationality)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.HomeLanguage)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.CitizenshipStatus)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Suburb)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.City)
                        .Include(a => a.Learner)*/
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Province) 
                        .Include(a => a.LearnerCourse)
                        .Include(a => a.JobApplications).ThenInclude(a => a.Job).ThenInclude(j => j.JobType)
                        .Include(a => a.JobApplications).ThenInclude(a => a.Job).ThenInclude(j => j.Sector)
                        /*.Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Country)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.AddressType)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.SchoolGrade)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.School)
                       */
                        .Select(a => new JobApplicationsDetailsDTO()
                        {
                            Email = a.Person.Email,
                            FirstName = a.Person.FirstName,
                            LastName = a.Person.LastName,
                            PersonsDob = a.Person.PersonsDob.ToString(Const.DATE_FORMAT),
                            GenderName = a.Person.Gender.GenderDesc,
                            EquityName = a.Person.Equity.EquityDesc,
                            HomeLanguage = a.Person.HomeLanguage.HomeLanguageDesc,
                            DisabilityStatus = a.Person.DisabilityStatus.DisabilityStatusDesc,
                            CitizenshipStatus = a.Person.CitizenshipStatus.CitizenshipStatusDesc,
                            PhoneNumber = a.Person.PhoneNumber,
                            Age = Utils.CalculateAge(a.Person.PersonsDob),
                            Nationality = a.Person.Nationality.NationalityDesc,
                            NationalID = a.Person.NationalId,
                            /*HouseNo = a.Person.Address.ToList()[0].HouseNo,
                            StreetName = a.Person.Address.ToList()[0].StreetName,
                            SuburbName = a.Person.Address.ToList()[0].Suburb.SuburbName,
                            CityName = a.Person.Address.ToList()[0].City.CityName,*/
                         //   ProvinceName = a.Person.Address.ToList()[0].Province.ProvinceName,
                            /*CountryName = a.Learner.Person.Address.ToList()[0].Country.CountryName,
                            AddressType = a.Learner.Person.Address.ToList()[0].AddressType.AddressTypeName,
                            PostalCode = a.Learner.Person.Address.ToList()[0].PostalCode,
                            SchoolName = a.Learner.School.SchoolName,
                            SchoolGradeName = a.Learner.SchoolGrade.SchoolGradeName,
                            YearSchoolCompleted = a.Learner.YearSchoolCompleted,*/
                            Applications =
                                (List<JobApplicationsDTO>) a.JobApplications.Select(jobApplications =>
                                    new JobApplicationsDTO()
                                    {
                                        LearnerId = jobApplications.LearnerId,
                                        JobId = jobApplications.JobId,
                                        DateApplied = jobApplications.DateApplied.ToString(Const.DATE_FORMAT),
                                        ExpiryDate = jobApplications.Job.ExpiryDate.ToString(Const.DATE_FORMAT),
                                        JobTitle = jobApplications.Job.JobTitle,
                                        SectorDesc = jobApplications.Job.Sector.SectorDesc,
                                        JobTypeDesc = jobApplications.Job.JobType.JobTypeDesc,
                                        JobApplicationStatus = jobApplications.ApplicationStatus
                                    })
                        }).ToListAsync();

                       return listOfJobApplications;
        }             
                
                public async Task<JobApplicationsDetailsDTO> GetJobApplicantPersonByNationalId(string id) 
                {

                    var listOfJobApplications = await _context.JobApplications.Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.Gender)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.Equity)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.DisabilityStatus)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.Nationality)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.HomeLanguage)
                        .ThenInclude(a => a.Person).ThenInclude(g => g.CitizenshipStatus)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Suburb)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.City)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Province)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.Country)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.Person).ThenInclude(a => a.Address).ThenInclude( s => s.AddressType)
                        .Include(a => a.Learner)
                        .ThenInclude(a => a.SchoolGrade)
                        .Include(a => a.Learner) 
                        .ThenInclude(a => a.School)  
                        .Include(a => a.Job)
                        .ThenInclude(a => a.Sector)
                        .Include(a => a.Job).ThenInclude(a => a.JobType).Where(a => a.Learner.Person.NationalId.Equals(id))
                        .Select(a => new JobApplicationsDetailsDTO()
                        {
                            Email = a.Learner.Person.Email,
                            FirstName = a.Learner.Person.FirstName,
                            LastName = a.Learner.Person.LastName,
                            PersonsDob = a.Learner.Person.PersonsDob.ToString(Const.DATE_FORMAT),
                            GenderName = a.Learner.Person.Gender.GenderDesc,
                            EquityName = a.Learner.Person.Equity.EquityDesc,
                            HomeLanguage = a.Learner.Person.HomeLanguage.HomeLanguageDesc,
                            DisabilityStatus = a.Learner.Person.DisabilityStatus.DisabilityStatusDesc,
                            CitizenshipStatus = a.Learner.Person.CitizenshipStatus.CitizenshipStatusDesc,
                            PhoneNumber = a.Learner.Person.PhoneNumber,
                            Age = Utils.CalculateAge(a.Learner.Person.PersonsDob),
                            Nationality = a.Learner.Person.Nationality.NationalityDesc,
                            NationalID = a.Learner.Person.NationalId,
                            HouseNo = a.Learner.Person.Address.ToList()[0].HouseNo,
                            StreetName = a.Learner.Person.Address.ToList()[0].StreetName,
                            SuburbName = a.Learner.Person.Address.ToList()[0].Suburb.SuburbName,
                            CityName = a.Learner.Person.Address.ToList()[0].City.CityName,
                            ProvinceName = a.Learner.Person.Address.ToList()[0].Province.ProvinceName,
                            CountryName = a.Learner.Person.Address.ToList()[0].Country.CountryName,
                            AddressType = a.Learner.Person.Address.ToList()[0].AddressType.AddressTypeName,
                            PostalCode = a.Learner.Person.Address.ToList()[0].PostalCode,
                            SchoolName = a.Learner.School.SchoolName,
                            SchoolGradeName = a.Learner.SchoolGrade.SchoolGradeName,
                            YearSchoolCompleted = a.Learner.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                            Applications =
                                (List<JobApplicationsDTO>) a.Learner.JobApplications.Select(jobApplications =>
                                    new JobApplicationsDTO()
                                    {
                                        LearnerId = jobApplications.LearnerId,
                                        JobId = jobApplications.JobId,
                                        DateApplied = jobApplications.DateApplied.ToString(Const.DATE_FORMAT),
                                        ExpiryDate = jobApplications.Job.ExpiryDate.ToString(Const.DATE_FORMAT),
                                        JobTitle = jobApplications.Job.JobTitle,
                                        SectorDesc = jobApplications.Job.Sector.SectorDesc,
                                        JobTypeDesc = jobApplications.Job.JobType.JobTypeDesc,
                                        JobApplicationStatus = jobApplications.ApplicationStatus
                                    }),
                            Qualifications = (List<QualificationDTO>) a.Learner.LearnerCourse.Select(qualification =>
                                new QualificationDTO()
                                {
                                    CourseName = qualification.CourseName,
                                    InstitutionName = qualification.InstitutionName,
                                    DateOfCompletion = qualification.DateOfCompletion
                                }),
                        }).FirstOrDefaultAsync();

                       return listOfJobApplications;
                }
             
        public async Task<Learner> GetPersonByNationalIdForEditDelete(string id) 
        { 
            var learner  = await _context.Learner  
                .Include(a => a.LearnerCourse)
                .Include(a => a.Person)
                .Include(a => a.Person.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Person.Address).ThenInclude(a => a.City)
                .Include(a => a.Person.Address).ThenInclude(a => a.Province)
                .Include(a => a.Person.Address).ThenInclude(a => a.Country)
                .Include(a => a.Person.Address).ThenInclude(a => a.AddressType) 
                .Include(a => a.Person.Equity) 
                .Include(a => a.Person.Gender)
                .Include(a => a.Person.Nationality)
                .Include(a => a.Person.CitizenshipStatus)
                .Include(a => a.Person.DisabilityStatus)
                .Include(a => a.Person.HomeLanguage)
                .Include(a => a.Documents).ThenInclude(d => d.DocumentType)
                .FirstOrDefaultAsync(p => p.Person.NationalId.Equals(id));

            return learner;
        }

  
        // Method Implementation for OFO
        
        public async Task<List<OfoDTO>> GetOFODetails() 
        {
            return await _context.Ofo.Include(a => a.Financialyear).Include(a => a.OfoUnit).Select( o => new OfoDTO()
            {
                Id = o.OfoId,
                OfoCode = o.OfoCode,
                OfoTitle = o.OfoTitle,
                FinancialyearName = o.Financialyear.FinancialyearDesc,
                OfoUnitName = o.OfoUnit.OfoUnitTitle
            }).ToListAsync();

        }


        public async Task<OfoDTO> GetOFODetailsById(long id)
        {
            return await _context.Ofo.Include(a => a.Financialyear).Include(a => a.OfoUnit).Select( o => new OfoDTO()
            {
                Id = o.OfoId,
                OfoCode = o.OfoCode,
                OfoTitle = o.OfoTitle,
                FinancialyearName = o.Financialyear.FinancialyearDesc,
                OfoUnitName = o.OfoUnit.OfoUnitTitle
            }).FirstOrDefaultAsync(o => o.Id.Equals(id));
        }        
     
        public async Task<Ofo> GetOFODetailsByIdForEditDelete(long id)
        {
            return await _context.Ofo.Include(a => a.Financialyear).Include(a => a.OfoUnit)
                .FirstOrDefaultAsync(o => o.OfoId.Equals(id));

        }
        
        
        public async Task<List<ProvinceDetailsDTO>> GetProvincesByCountryId(long id) 
        {
            return await _context.Province.Include(a => a.Country).Where(a => a.CountryId == id).Select( a => new ProvinceDetailsDTO()
            {
                Id = a.ProvinceId,
                CountryName = a.Country.CountryName,
                ProvinceName = a.ProvinceName,
                ProvinceCode = a.ProvinceCode
            }).ToListAsync();
            
        }

        public async Task<List<CityDetailsDTO>> GetCitiesByProvincesId(long id)
        {
            return await _context.City.Include(a => a.Province).Where(a => a.ProvinceId == id).Select( a => new CityDetailsDTO()
            {
                Id = a.CityId,
                CityName = a.CityName,
                CityCode = a.CityCode,
                ProvinceName = a.Province.ProvinceName
            }).ToListAsync();
        }
        
        
        public async Task<List<SuburbsDetailsDTO>> GetSuburbsByCityId(long id)
        {
            return await _context.Suburb.Include(a => a.City).Where(a => a.CityId == id).Select( a => new SuburbsDetailsDTO()
            {
                Id = a.SuburbId,
                CityName = a.City.CityName,
                SuburbName = a.SuburbName,
                SuburbCode = a.SuburbCode
            }).ToListAsync();
        }


        // Method Implementation for OFO Unit

        public async Task<List<OfoUnitDTO>> GetOfoUnitDetails()
        {
            return await _context.OfoUnit.Include(m => m.OfoMinor).Include(f => f.Financialyear).Select ( ou => new OfoUnitDTO()
            {
                Id = ou.OfoUnitId,
                OfoUnitCode = ou.OfoUnitCode,
                OfoUnitTitle = ou.OfoUnitTitle,
                OfoMinorTitle = ou.OfoMinor.OfoMinorTitle,
                FinancialYearDesc = ou.Financialyear.FinancialyearDesc
                }).ToListAsync();
         
        }


        public async Task<OfoUnitDTO> GetOFOUnitDetailsById(long id)
        {
           return await _context.OfoUnit.Include(m => m.OfoMinor).Include(f => f.Financialyear).Where(o => o.OfoUnitId.Equals(id) ).Select(ou =>  new OfoUnitDTO()
                             {
                                 Id = ou.OfoUnitId,
                                 OfoUnitCode = ou.OfoUnitCode,
                                 OfoUnitTitle = ou.OfoUnitTitle,
                                 OfoMinorTitle = ou.OfoMinor.OfoMinorTitle,
                                 FinancialYearDesc = ou.Financialyear.FinancialyearDesc
                             }).FirstOrDefaultAsync();

      
        }

        public async Task<OfoUnit> GetOFOUnitDetailsByIdForEditDelete(long id)
        {
            return await _context.OfoUnit.Include(m => m.OfoMinor).Include(f => f.Financialyear).Where( o => o.OfoUnitId.Equals(id))
                                .FirstOrDefaultAsync();
        }
        


        // Method Implementation for OFO Unit

        public async Task<List<OfoMinorDTO>> GetOfoMinorDetails()
        {
            return await _context.OfoMinor.Include(a => a.Financialyear).Select(o => new OfoMinorDTO
            {
                Id = o.OfoMinorId,
                FinancialYearDesc = o.Financialyear.FinancialyearDesc,
                OfoMinorCode = o.OfoMinorCode,
                OfoMinorTitle = o.OfoMinorTitle
            }).ToListAsync();
        }

        public async Task<OfoMinorDTO> GetOfoMinorById(long id)
        {
            return await _context.OfoMinor.Include(a => a.Financialyear).Where(o => o.OfoMinorId.Equals(id)).Select(o => new OfoMinorDTO
            {
                Id = o.OfoMinorId,
                FinancialYearDesc = o.Financialyear.FinancialyearDesc,
                OfoMinorCode = o.OfoMinorCode,
                OfoMinorTitle = o.OfoMinorTitle

            }).FirstOrDefaultAsync();
            
        }

        public async Task<OfoMinor> GetOfoMinorByIdForEditDelete(long id)
        {
            return await _context.OfoMinor.Include(a => a.Financialyear).Where(o => o.OfoMinorId.Equals(id)).FirstOrDefaultAsync();
        }


        // Method Implementation for School
         
        public async Task<List<SchoolDTO>> GetSchoolDetails()
        {
            return await _context.School.Select( s => new SchoolDTO()
                {
                    Id = s.SchoolId,
                    SchoolCode = s.SchoolCode,
                    EmisNo = s.EmisNo,
                    SchoolName = s.SchoolName
                }).ToListAsync();

        }


        public async Task<SchoolDTO> GetSchoolDetailsById(long id)
        {
            return await _context.School.Where(s => s.SchoolId.Equals(id)).Select(s => new SchoolDTO()
            {
                Id = s.SchoolId,
                SchoolCode = s.SchoolCode,
                EmisNo = s.EmisNo,
                SchoolName = s.SchoolName
            }).FirstOrDefaultAsync();

        }


        public async Task<School> GetSchoolByIdForEditDelete(long id)
        {
            return await _context.School.Where(s => s.SchoolId.Equals(id)).FirstOrDefaultAsync();

        }

        // Method Implementation for Job

        public async  Task<List<JobDetailsDTO>> GetJobDetails()
        {
            return await _context.Jobs
                .Include(s => s.Sector)
                .Include(t => t.JobType)
                .Include(o => o.Ofo)
                .Include(p => p.Province)
                .Include(c => c.Company).Select( j => new JobDetailsDTO()
                {
                    Id = j.JobId,
                    JobCode = j.JobCode,
                    JobTitle = j.JobTitle,
                    JobDesc = j.JobDesc,
                    ListedDate = j.ListedDate,
                    ExpiryDate = j.ExpiryDate,
                    SectorDesc = j.Sector.SectorDesc,
                    JobTypeDesc = j.JobType.JobTypeDesc,
                    OfoTitle = j.Ofo.OfoTitle,
                    ProvinceName = j.Province.ProvinceName,
                    CompanyName = j.Company.CompanyName
                    
                }).ToListAsync();

            
        }
        public async Task<JobDetailsDTO> GetJobDetailsById(long id)
        {
       return await  _context.Jobs.Include(s => s.Sector)
           .Include(t => t.JobType)
           .Include(o => o.Ofo)
           .Include(p => p.Province)
           .Include(c => c.Company)
           .Where(j => j.JobId == id ).Select( j => new JobDetailsDTO()
                {
                    Id = j.JobId,
                    JobCode = j.JobCode,
                    JobTitle = j.JobTitle,
                    JobDesc = j.JobDesc,
                    ListedDate = j.ListedDate,
                    ExpiryDate = j.ExpiryDate,
                    SectorDesc = j.Sector.SectorDesc,
                    JobTypeDesc = j.JobType.JobTypeDesc,
                    OfoTitle = j.Ofo.OfoTitle,
                    ProvinceName = j.Province.ProvinceName,
                    CompanyName = j.Company.CompanyName
                }).FirstOrDefaultAsync();

        }
        

        public async Task<Job> GetJobDetailsByIdForEditDelete(long id) 
        {
            return await _context.Jobs
                .Include(s => s.Sector)
                .Include(t => t.JobType)
                .Include(o => o.Ofo)
                .Include(p => p.Province)
                .Include(c => c.Company)
                .Where(s => s.JobId ==id).FirstOrDefaultAsync();
        }
        
        
        //Method Implementation for Countries
        
        public async Task<List<CountriesDetailsDTO>> GetCountriesDetails()   
        {
            
            var countryDetails =
                await _context.Country
                    .Select(c => new CountriesDetailsDTO()
                    {
                        Id = c.CountryId,
                        CountryCode = c.CountryCode,
                        CountryName = c.CountryName
                    }).ToListAsync(); 
 
            return  countryDetails; 
        }
        
        public async Task<CountriesDetailsDTO> GetCountriesDetailsById(long id)
        {
            return await  _context.Country
                .Where(c => c.CountryId == id ).Select( c => new CountriesDetailsDTO()
                {
                    Id = c.CountryId,
                    CountryCode = c.CountryCode,
                    CountryName = c.CountryName
                }).FirstOrDefaultAsync();

        }
        
        
        public async Task<Country> GetCountriesDetailsByIdForEditDelete(long id) 
        {
            return await _context.Country
                .Where(c => c.CountryId == id).FirstOrDefaultAsync();
        }
        
        // Method Implementation for Province
        
        public async Task<List<ProvinceDetailsDTO>> GetProvincesDetails()
        {
            var provinceDetails =
                await _context.Province.Include(p => p.Country)
                    .Select(p => new ProvinceDetailsDTO()
                    {
                        Id = p.ProvinceId,
                        ProvinceCode = p.ProvinceCode,
                        ProvinceName = p.ProvinceName,
                        CountryName = p.Country.CountryName,

                    }).ToListAsync(); 
 
            return  provinceDetails; 
        }
        
        public async Task<ProvinceDetailsDTO> GetAllProvincesById(long id)
        {
            return await  _context.Province.Include(p => p.Country)
                .Where(p => p.ProvinceId == id ).Select( p => new ProvinceDetailsDTO()
                {
                    Id = p.ProvinceId,
                    ProvinceCode = p.ProvinceCode,
                    ProvinceName = p.ProvinceName,
                    CountryName = p.Country.CountryName
                }).FirstOrDefaultAsync();

        }
        
        public async Task<Province> GetProvinceDetailsByIdForEditDelete(long id) 
        {
            return await _context.Province.Include(p => p.Country)
                .Where(p => p.ProvinceId == id).FirstOrDefaultAsync();
        }
        

// Method Implementation for City
        public async Task<List<CityDetailsDTO>> GetCitiesDetails()
        {
            var CityDetails =
                await _context.City.Include(c => c.Province)
                    .Select(c => new CityDetailsDTO()
                    {
                        Id = c.CityId,
                        CityCode = c.CityCode,
                        CityName = c.CityName,
                        ProvinceName = c.Province.ProvinceName,

                    }).ToListAsync(); 
 
            return  CityDetails; 
        }
        
        public async Task<CityDetailsDTO> GetAllCitiesById(long id)
        {
            return await  _context.City.Include(c => c.Province)
                .Where(c => c.CityId == id ).Select( c => new CityDetailsDTO()
                {
                    Id = c.CityId,
                    CityCode = c.CityCode,
                    CityName = c.CityName,
                    ProvinceName = c.Province.ProvinceName,
                }).FirstOrDefaultAsync();

        }
        
        public async Task<City> GetCitiesDetailsByIdForEditDelete(long id) 
        {
            return await _context.City.Include(p => p.Province)
                .Where(p => p.CityId == id).FirstOrDefaultAsync();
        }
        
        
        // Method Implementation for Suburb
        public async Task<List<SuburbsDetailsDTO>> GetSuburbsDetails()
        {
            var SuburbDetails =
                await _context.Suburb.Include(s => s.City)
                    .Select(s => new SuburbsDetailsDTO()
                    {
                        Id = s.SuburbId,
                       SuburbCode = s.SuburbCode,
                       SuburbName = s.SuburbName,
                       CityName = s.City.CityName,

                    }).ToListAsync(); 
 
            return  SuburbDetails; 
        }
        
        public async Task<SuburbsDetailsDTO> GetAllSuburbsById(long id)
        {
            return await  _context.Suburb.Include(s => s.City)
                .Where(s => s.SuburbId == id ).Select( s => new SuburbsDetailsDTO()
                {
                    Id = s.SuburbId,
                    SuburbCode = s.SuburbCode,
                    SuburbName = s.SuburbName,
                    CityName = s.City.CityName,
                }).FirstOrDefaultAsync();

        }
        
        public async Task<Suburb> GetSuburbsDetailsByIdForEditDelete(long id) 
        {
            return await _context.Suburb.Include(p => p.City)
                .Where(p => p.SuburbId == id).FirstOrDefaultAsync();
        }
        
        // Method Implementation for Address Type
        public async Task<List<AddressTypeDetailsDTO>> GetAddressTypeDetails()
        {
            var addressTypeDetails =
                await _context.AddressType
                    .Select(a => new AddressTypeDetailsDTO()
                    {
                        Id = a.AddressTypeId, 
                        AddressTypeCode = a.AddressTypeCode,
                        AddressTypeName = a.AddressTypeName

                    }).ToListAsync();
            return  addressTypeDetails; 
        }
        
        public async Task<AddressTypeDetailsDTO> GetAllAddressTypeById(long id)
        {
            return await  _context.AddressType
                .Where(s => s.AddressTypeId == id ).Select( a => new AddressTypeDetailsDTO()
                {
                    Id = a.AddressTypeId, 
                    AddressTypeCode = a.AddressTypeCode,
                    AddressTypeName = a.AddressTypeName
                }).FirstOrDefaultAsync();

        }
        
        public async Task<AddressType> GetAddressTypeDetailsByIdForEditDelete(long id)
        {
            return await _context.AddressType
                .Where(p => p.AddressTypeId == id).FirstOrDefaultAsync();
        }
        
        
        // Method Implementation for Document Type
        public async Task<List<DocumentTypesDetailsDTO>> GetDocumentTypesDetails()
        {
            var documentTypesDetails =
                await _context.DocumentType.Include(r => r.Role)
                    .Select(d => new DocumentTypesDetailsDTO()
                    {
                        Id = d.Id,
                        TypeName = d.TypeName,
                        Description = d.Description,
                        RoleName = d.Role.Name,
                        ActiveYn = d.ActiveYn

                    }).ToListAsync(); 
 
            return  documentTypesDetails; 
        }


        public async Task<List<DocumentTypesDetailsDTO>> GetDocumentTypesDetailsByRole(string role)
        {
            var documentTypesDetails =
                await _context.DocumentType.Where(a => a.Role.Name.Equals(role)).Include(r => r.Role)
                    .Select(d => new DocumentTypesDetailsDTO()
                    {
                        Id = d.Id,
                        TypeName = d.TypeName,
                        Description = d.Description,
                        RoleName = d.Role.Name,
                        ActiveYn = d.ActiveYn

                    }).ToListAsync(); 
 
            return  documentTypesDetails; 
        }
        
        public async Task<DocumentTypesDetailsDTO> GetAllDocumentTypesById(Guid id)
        {
            return await  _context.DocumentType.Include(r => r.Role)
                .Where(d => d.Id == id ).Select( d => new DocumentTypesDetailsDTO()
                {
                    Id = d.Id,
                    TypeName = d.TypeName,
                    Description = d.Description,
                    RoleName = d.Role.Name,
                    ActiveYn = d.ActiveYn
                }).FirstOrDefaultAsync();

        }
        
        public async Task<DocumentType> GetDocumentTypesDetailsByIdForEditDelete(Guid id) 
        {
            return await _context.DocumentType
                .Where(d => d.Id == id).FirstOrDefaultAsync();
        }
        
        
        
        // Method Implementation for Document
        public async Task<List<DocumentDetailsDTO>> GetDocumentsDetails()
        {
            var documentDetails =
                await _context.Document
                    .Include(t => t.DocumentType)
                    .Include(t => t.Company)
                    .Include(t => t.Learner)
                    .Include(t => t.JobApplications)
                    
                    .Select(d => new DocumentDetailsDTO()
                    {
                        Id = d.Id,
                        DocumentTypeName = d.DocumentType.TypeName,
                        Comment = d.Comments,
                        LearnerId = (d.Learner.Person.FirstName + " " + d.Learner.Person.LastName),
                        CompanyName = d.Company.CompanyName,
                        Verified = d.Verified,
                        VerifiedBy = d.VerifiedBy,
                        VerificationDate = d.VerificationDate,
                        JobApplication = new Guid(d.JobApplications.ApplicationStatus)


                    }).ToListAsync(); 
 
            return  documentDetails; 
        }
        
        
        public async Task<DocumentDetailsDTO> GetDocumentById(Guid id)
        {
            return await  _context.Document
                .Include(t => t.DocumentType)
                .Include(t => t.Company)
                .Include(t => t.Learner)
                .Include(t => t.JobApplications)
                .Where(d => d.Id ==id ).Select( d => new DocumentDetailsDTO()
                {

                    Id = d.Id,
                    DocumentTypeName = d.DocumentType.TypeName,
                    Comment = d.Comments,
                    LearnerId = d.Learner.Person.FirstName,
                    CompanyName = d.Company.CompanyName,
                    Verified = d.Verified,
                    VerifiedBy = d.VerifiedBy,
                    VerificationDate = d.VerificationDate,
                    JobApplication = new Guid(d.JobApplications.ApplicationStatus)
                    
                }).FirstOrDefaultAsync();

        }

        public async Task<Document> GetDocumentsDetailsByIdForEditDelete(Guid id) 
        {
            return await _context.Document
                .Include(t => t.DocumentType)
                .Include(t => t.Company)
                .Include(t => t.Learner)
                .Include(t => t.JobApplications)
                .Where(d => d.Id == id).FirstOrDefaultAsync();
        }
        
        

// Method Implementation for Institution
        
        public async Task<List<InstitutionDetailsDTO>> GetInstitutionDetails()
        {
            return await _context.Institution.Include(t => t.InstitutionType).Select(I =>  new InstitutionDetailsDTO()
                {
                    Id = I.InstitutionId,
                    InstitutionCode = I.InstitutionCode,
                    InstitutionName = I.InstitutionName,
                    InstitutionTypeDesc = I.InstitutionType.InstitutionTypeDesc
                }).ToListAsync();
        }

        
        public  async Task<InstitutionDetailsDTO> GetInstitutionDetailsById(long id)
        {
              var institutionDetails =  await _context.Institution.Include(t => t.InstitutionType).Where(I => I.InstitutionId.Equals(id))
                  .Select(I => new InstitutionDetailsDTO()
                  {
                      Id = I.InstitutionId,
                      InstitutionCode = I.InstitutionCode,
                      InstitutionName = I.InstitutionName,
                      InstitutionTypeDesc = I.InstitutionType.InstitutionTypeDesc
                  }).FirstOrDefaultAsync();
 
            return institutionDetails;
        }
        
        
        public async Task<Institution> GetInstitutionByIdForEditDelete(long id)
        {
            return await _context.Institution.Include(t => t.InstitutionType).Where(I => I.InstitutionId.Equals(id)).FirstOrDefaultAsync();
        }
        

        // Method Implementation for Learner

        public async Task<LearnerDetailsDto> GetLearnerDetailsByIdEmail( string id)
        {
           return await _context.Learner
                .Include(a => a.School)
                .Include(a => a.SchoolGrade)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.City)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.City)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.Suburb)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.Province)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.Country)
                .Include(a => a.Person).ThenInclude(a =>a.Address).ThenInclude(a => a.AddressType)
                .Include(a => a.Person).ThenInclude(a =>a.CitizenshipStatus)
                .Include(a => a.Person).ThenInclude(a =>a.Equity)
                .Include(a => a.Person).ThenInclude(a =>a.Gender)
                .Include(a => a.Person).ThenInclude(a =>a.Nationality)
                .Include(a => a.Person).ThenInclude(a =>a.HomeLanguage)
                .Include(a => a.Person).ThenInclude(a =>a.DisabilityStatus)
                .Where(a => a.Person.NationalId.Equals(id) || a.Person.Email.Equals(id)).Select( learner => new LearnerDetailsDto()
                {
                   LearnerId = learner.LearnerId,
                    FirstName = learner.Person.FirstName,
                    LastName = learner.Person.LastName,
                    Email = learner.Person.Email,
                    PhoneNumber = learner.Person.PhoneNumber,
                    PersonsDob = learner.Person.PersonsDob.ToString(Const.DATE_FORMAT), 
                    Age = Utils.CalculateAge(learner.Person.PersonsDob), 
                    NationalID = learner.Person.NationalId,
                    EquityName = learner.Person.Equity.EquityDesc,
                    DisabilityStatus = learner.Person.DisabilityStatus.DisabilityStatusDesc,
                    HomeLanguage = learner.Person.HomeLanguage.HomeLanguageDesc,
                    CitizenshipStatus = learner.Person.CitizenshipStatus.CitizenshipStatusDesc,
                    Nationality = learner.Person.Nationality.NationalityDesc,
                    GenderName = learner.Person.Gender.GenderDesc,
                    HouseNo = learner.Person.Address.ToList()[0].HouseNo ,
                    StreetName = learner.Person.Address.ToList()[0].StreetName , 
                    PostalCode =learner.Person.Address.ToList()[0].PostalCode,
                    CountryName = learner.Person.Address.ToList()[0].Country.CountryName,
                    CityName = learner.Person.Address.ToList()[0].City.CityName,
                    SuburbName = learner.Person.Address.ToList()[0].Suburb.SuburbName,
                    ProvinceName = learner.Person.Address.ToList()[0].Province.ProvinceName,
                    AddressType = learner.Person.Address.ToList()[0].AddressType.AddressTypeName,
                    PhotoName = learner.Person.PhotoName,
                    PhotoPath = learner.Person.PhotoPath,
                    MotivationText = learner.MotivationText,
                    SchoolName = learner.School.SchoolName,
                    SchoolGradeName = learner.SchoolGrade.SchoolGradeName,
                    YearSchoolCompleted = learner.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                    Qualifications = learner.LearnerCourse.Select(a => new QualificationDTO()
                    {
                        CourseName = a.CourseName,
                        InstitutionName = a.InstitutionName,
                        DateOfCompletion = a.DateOfCompletion
                    }).ToList()}
                ).FirstOrDefaultAsync();
            
        }


        // Method Implementation for Job Applications

        public async Task<JobApplicationsDetailsDTO> GetJobApplicationsById(string id)
        {
            var listOfJobApplications = await _context.JobApplications.Include(a => a.Learner)
                .ThenInclude(a => a.Person)
                .ThenInclude(a => a.Address)
                .ThenInclude(p => p.Province)
                .Include(a => a.Job)
                .Where(a => a.Learner.Person.NationalId.Equals(id))
                .Select(a => new JobApplicationsDetailsDTO()
                {
                    Email = a.Learner.Person.Email,
                    FirstName = a.Learner.Person.FirstName,
                    LastName = a.Learner.Person.LastName,
                    PersonsDob = a.Learner.Person.PersonsDob.ToString(Const.DATE_FORMAT),
                    GenderName = a.Learner.Person.Gender.GenderDesc,
                    EquityName = a.Learner.Person.Equity.EquityDesc,
                    HomeLanguage = a.Learner.Person.HomeLanguage.HomeLanguageDesc,
                    DisabilityStatus = a.Learner.Person.DisabilityStatus.DisabilityStatusDesc,
                    CitizenshipStatus = a.Learner.Person.CitizenshipStatus.CitizenshipStatusDesc,
                    PhoneNumber = a.Learner.Person.PhoneNumber,
                    Age = Utils.CalculateAge(a.Learner.Person.PersonsDob),
                    Nationality = a.Learner.Person.Nationality.NationalityDesc,
                    NationalID = a.Learner.Person.NationalId,
                    HouseNo = a.Learner.Person.Address.ToList()[0].HouseNo,
                    StreetName = a.Learner.Person.Address.ToList()[0].StreetName,
                    SuburbName = a.Learner.Person.Address.ToList()[0].Suburb.SuburbName,
                    CityName = a.Learner.Person.Address.ToList()[0].City.CityName,
                    ProvinceName = a.Learner.Person.Address.ToList()[0].Province.ProvinceName,
                    CountryName = a.Learner.Person.Address.ToList()[0].Country.CountryName,
                    AddressType = a.Learner.Person.Address.ToList()[0].AddressType.AddressTypeName,
                    PostalCode = a.Learner.Person.Address.ToList()[0].PostalCode,
                    SchoolName = a.Learner.School.SchoolName,
                    SchoolGradeName = a.Learner.SchoolGrade.SchoolGradeName,
                    YearSchoolCompleted = a.Learner.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                    Applications =
                        (List<JobApplicationsDTO>) a.Learner.JobApplications.Select(jobApplications =>
                            new JobApplicationsDTO()
                            {
                                DateApplied = jobApplications.DateApplied.ToString(Const.DATE_FORMAT),
                                ExpiryDate = jobApplications.Job.ExpiryDate.ToString(Const.DATE_FORMAT),
                                JobTitle = jobApplications.Job.JobTitle,
                                SectorDesc = jobApplications.Job.Sector.SectorDesc,
                                JobTypeDesc = jobApplications.Job.JobType.JobTypeDesc,
                                JobApplicationStatus = jobApplications.ApplicationStatus
                            }),
                    Qualifications = (List<QualificationDTO>) a.Learner.LearnerCourse.Select(qualification =>
                        new QualificationDTO()
                        {
                            CourseName = qualification.CourseName,
                            InstitutionName = qualification.InstitutionName,
                            DateOfCompletion = qualification.DateOfCompletion
                        }),
                })
                .FirstOrDefaultAsync();
        
            return listOfJobApplications;
        } 
        
        //Get Country Provinces
        public List<Province> GetAllProvinces(long id)
        {
            var ListOfProvinces = new List<Province>();
            ListOfProvinces = (from province in _context.Province where province.CountryId == id select province).ToList();
            ListOfProvinces.Insert(0, new Province {ProvinceId = 0, ProvinceName = "Select"});

            return ListOfProvinces;
        }

        #endregion
    }
}
