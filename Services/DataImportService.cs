using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelDataReader;
using learner_portal.Helpers;
using learner_portal.Models;
using Microsoft.Extensions.Logging;

namespace learner_portal.Services
{
    public class DataImportService : IDataImportService
    {
        private readonly LearnerContext _learnerContext;
        private readonly ILookUpService _lookUpService;
        private readonly ILogger<DataImportService> _logger;  

        public DataImportService(ILookUpService lookUpService,LearnerContext learnerContext,ILogger<DataImportService> logger)
        {
            _lookUpService = lookUpService;
            _learnerContext = learnerContext;
            _logger = logger;

        }


        public async Task<bool> ImportExcelForLearners(string fileName)
        {

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            await using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = false,
                        }
                    };
                    
                    var dataSet = reader.AsDataSet(conf);

                    var dataTable = dataSet.Tables[0];

     
                    for (var i = 1; i < dataTable.Rows.Count; i++)//Each row of the file
                    {
                        var person = new Person()
                        {
                            NationalId = dataTable.Rows[i].ItemArray[0].ToString(),
                            Title = dataTable.Rows[i].ItemArray[1].ToString(),
                            FirstName = dataTable.Rows[i].ItemArray[2].ToString(),
                            LastName = dataTable.Rows[i].ItemArray[3].ToString(),
                            PersonsDob = DateTime.ParseExact(dataTable.Rows[i].ItemArray[4].ToString(), "yyyy/MM/dd HH:mm:ss", CultureInfo.CurrentCulture),
                            GenderId = (await _lookUpService.GetGenders()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[5].ToString()))?.id,
                            NationalityId =  (await  _lookUpService.GetNationalities()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[6].ToString()))?.id,
                            CitizenshipStatusId = (await  _lookUpService.GetCitizenships()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[7].ToString()))?.id,
                            DisabilityStatusId = (await  _lookUpService.GetDisabilities()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[8].ToString()))?.id,
                            HomeLanguageId =  (await _lookUpService.GetHomeLanguages()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[9].ToString()))?.id,
                            EquityId =   (await _lookUpService.GetEquities()).Find(a => a.name.Equals(dataTable.Rows[i].ItemArray[10].ToString()))?.id,
                            Email = dataTable.Rows[i].ItemArray[11].ToString(),
                            PhoneNumber = dataTable.Rows[i].ItemArray[12].ToString(),
                            CreatedBy = "DataImport",
                            DateCreated = DateTime.Now,
                           
                        };

                        var homeAddress = new Address()
                        {
                            HouseNo = dataTable.Rows[i].ItemArray[13].ToString(),
                            StreetName = dataTable.Rows[i].ItemArray[14].ToString(),
                            SuburbId =  _lookUpService.GetSuburbs().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[15].ToString()))?.id,
                            CityId =  _lookUpService.GetCities().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[16].ToString()))?.id,
                            ProvinceId = _lookUpService.GetProvinces().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[17].ToString()))?.id,
                            CountryId =  _lookUpService.GetCountries().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[18].ToString()))?.id,
                            PostalCode = dataTable.Rows[i].ItemArray[19].ToString(),
                            AddressTypeId = 1,
                            CreatedBy = "DataImport",
                            DateCreated = DateTime.Now,
                        };         
                        
                        var postalAddress = new Address()
                        {
                            HouseNo = dataTable.Rows[i].ItemArray[20].ToString(),
                            StreetName = dataTable.Rows[i].ItemArray[21].ToString(),
                            SuburbId =  _lookUpService.GetSuburbs().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[22].ToString()))?.id,
                            CityId =  _lookUpService.GetCities().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[23].ToString()))?.id,
                            ProvinceId = _lookUpService.GetProvinces().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[24].ToString()))?.id,
                            CountryId =  _lookUpService.GetCountries().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[25].ToString()))?.id,
                            PostalCode = dataTable.Rows[i].ItemArray[26].ToString(),
                            AddressTypeId = 1,
                            CreatedBy = "DataImport",
                            DateCreated = DateTime.Now,
                            
                        };
                        
                        person.Address.Add(homeAddress);
                        person.Address.Add(postalAddress);

                        var learner = new Learner
                        {
                            SchoolId =  _lookUpService.GetSchools().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[27].ToString())).id,
                            SchoolGradeId =  _lookUpService.GetSchoolGrades().Result.FirstOrDefault(a => a.name.Equals(dataTable.Rows[i].ItemArray[28].ToString())).id,
                            YearSchoolCompleted = Utils.GetDate(dataTable.Rows[i].ItemArray[29].ToString()),
                            AppliedYn = Const.FALSE,
                            RecruitedYn = Const.FALSE,
                            CreatedBy = "DataImport",
                            DateCreated = DateTime.Now,
                            Person = person,
                        };
                        learner.Person = person;
                        
                        _learnerContext.Learner.Add(learner);
                        await _learnerContext.SaveChangesAsync();
                    }
                }
            }
            return true;
        }
    }
}