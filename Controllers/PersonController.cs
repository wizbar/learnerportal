using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using learner_portal.DTO;
using learner_portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Rotativa.AspNetCore;

namespace learner_portal.Controllers
{
    
    [Authorize]
    public class PersonController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly ILookUpService _lookUpService;
        private readonly IFileService  _fileService;
        private readonly UserManager<Users> _userManager;
        private readonly FoldersConfigation _fconfig;
        private readonly INotyfService _notyf;
        public PersonController(LearnerContext context, IWebHostEnvironment env,
                                 ILookUpService lookUpService,IFileService fileService,
                                 FoldersConfigation fconfig,
                                 UserManager<Users> userManager,
                                 INotyfService notyf
                                 )
        {
            _context = context;
            _env = env;
            _lookUpService = lookUpService;
            _fileService = fileService;
            _fconfig = fconfig;
            _userManager = userManager;
            _notyf = notyf;
        }

        public JsonResult GetCountryProvinces(long id)
        {
            return Json(_lookUpService.GetProvincesByCountryId(id));
        }
  
        // GET: Person
        public  IActionResult Index()
        {
                   
            return View().WithSuccess("It worked!", "You were able to view the about page, congrats!");
        }
        
        public async Task<JsonResult> GetAllPerson()
        { 
            try   
            { 
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Query["start"].FirstOrDefault();
                // Paging Length 10,20   
                var length = Request.Query["length"].FirstOrDefault();
                // Sort Column Name  
                var sortColumn = Request
                    .Query["columns[" + Request.Query["order[0][column]"].FirstOrDefault() + "][name]"]
                    .FirstOrDefault();
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Query["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                
               var listOfPerson = await _lookUpService.GetPersonDetails().ConfigureAwait(false);

                // Getting all Customer data  z 
                var allPerson = listOfPerson;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allPerson = allPerson.Where(m =>
                            m.FirstName == searchValue ||
                            m.LastName == searchValue || 
                            m.GenderName == searchValue)
                        as List<PersonDetailsDTO>;
                }

                //total number of rows count   
                recordsTotal = allPerson.Count();
                //Paging   
                var data = allPerson.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allPerson });
            }
            catch (Exception)
            {
                throw;
            }  
        }

        // GET: Person/Details/5
        public async Task<IActionResult> Details(string id)
        {
           if (id == null)
           {
               return NotFound();
           }

           var personDetails = await _lookUpService.GetPersonByNationalId(id);
            
           if (personDetails == null)
           { 
               return NotFound();
           }
           var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
           var userRole =  _userManager.GetRolesAsync(user).Result;
           
           ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
           return View(personDetails);
        }
  
        // GET: Person/Create
        public IActionResult Create()
        {
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusId");
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusId");
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityId");
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderId");
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageId");
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityId");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Person/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,PersonsDob,GenderId,NationalId,EquityId,NationalityId,HomeLanguageId,CitizenshipStatusId,DisabilityStatusId,UserId,PhotoPath,PhotoName,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Person person)
        {
            if (ModelState.IsValid)
            {
                
                _context.Add(person);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusId", person.CitizenshipStatusId);
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusId", person.DisabilityStatusId);
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityId", person.EquityId);
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderId", person.GenderId);
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageId", person.HomeLanguageId);
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityId", person.NationalityId);
 
            return View(person);
        }

        // Create PDF and get Person's details.
     

        public async Task<IActionResult> ViewResumePdf(string id)
        {

            // Checks if the id.
            if (string.IsNullOrEmpty(id))
            {
                return NotFound(); 
            }

            string path;
            byte[] fileBytes = null;    
            if (ModelState.IsValid)
            {
                try
                {

                   path =  await CreateResumeDocument(id);
                   fileBytes = System.IO.File.ReadAllBytes(path);  
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return File(fileBytes, "application/pdf"); 
        }
        
        [HttpGet]
        public async Task<string> CreateResumeDocument(string id)
        { 
            
            var personDetails = await _lookUpService.GetLearnerDetailsByIdEmail(id);

            //Convert HTML file to PDF and store the bytes in an pdf variable and attache a filename.
            var pdf = new ViewAsPdf("ViewResumePdf", personDetails)
            {
                FileName = personDetails.NationalID + ".pdf",
                /*CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"*/
            }; 
             
            //Extract bytes of the created PDF document
            var pdfData = await pdf.BuildFile(ControllerContext);

            //Create the PDF file name and attach  the director where the document will be saved
            var wwwwRootPath = _env.WebRootPath;
            var fileName = Path.GetFileNameWithoutExtension(pdf.FileName);
            var extention = Path.GetExtension(pdf.FileName);
            var newFileName = fileName + DateTime.Now.ToString("_yymmddssfff") + extention;
            var fullPath = Path.Combine(wwwwRootPath + "/Documents/", newFileName);

            //Save the PDF document (physical file) using the PDF bytes extracted and  file path and name created above
            using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(pdfData, 0, pdfData.Length);
            } 
            
  
            return fullPath;
        }

        // Create Account
        public async Task<IActionResult> CreateAccount()
        {
          
                                   
            /*Message("User logged in",Enum.NotificationType.success);
            Alert("User logged in",Enum.NotificationType.error);*/
            
            
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc");
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc");
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc");
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc");
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc");
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc");
            ViewData["CityId"] = new SelectList(await _lookUpService.GetCities(), "id", "name");
            ViewData["SuburbId"] = new SelectList(_lookUpService.GetSuburbs().Result, "id", "name");
            ViewData["CountryId"] = new SelectList(_lookUpService.GetCountries().Result, "id", "name");
            ViewData["ProvinceId"] = new SelectList(_lookUpService.GetProvinces().Result, "id", "name");
            ViewData["AddressTypeId"] = new SelectList(_lookUpService.GetAddressTypes().Result, "id", "name");
            
            /*ViewData["InstitutionId"] = new SelectList(_lookUpService.GetInstitutions().Result, "id", "name");
            ViewData["CourseId"] = new SelectList(_lookUpService.GetCourses().Result, "id", "name");*/
            ViewData["SchoolId"] = new SelectList(_lookUpService.GetSchools().Result, "id", "name");
            ViewData["SchoolGradeId"] = new SelectList(_lookUpService.GetSchoolGrades().Result, "id", "name");
        
            
            

         return View();
        }
         
        //Create Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount([Bind("Person", "Address", "Photo", "Learner", "Cv")] LearnerViewModel learnerViewModel)
        {
            if (ModelState.IsValid)
            { 
                // This Uploads learner profile image
                var photoPath =  _fconfig.Images  + learnerViewModel.Person.NationalId + "/" + Utils.GenerateImageFolderId() + "/";
               
                if (learnerViewModel.Photo != null)
                { 
                   _fileService.UploadFile( learnerViewModel.Photo,_env.WebRootPath + photoPath);    
                    
                   learnerViewModel.Person.PhotoName = learnerViewModel.Photo.FileName; 
                   learnerViewModel.Person.PhotoPath = photoPath;
                   Console.WriteLine(" FILE NAME : " + learnerViewModel.Photo.FileName); 
                }
                
                var cvPath =  _fconfig.Documents  + learnerViewModel.Person.NationalId + "/" + Utils.GenerateDocsFolderId() + "/";

                if (learnerViewModel.Cv != null)
                { 
                    _fileService.UploadFile( learnerViewModel.Cv,_env.WebRootPath + cvPath);    
                    
                    learnerViewModel.Person.CvName = learnerViewModel.Cv.FileName; 
                    learnerViewModel.Person.CvPath = cvPath;
                    Console.WriteLine(" FILE NAME : " + learnerViewModel.Cv.FileName); 
                }
  
                learnerViewModel.Person.CreatedBy = "admin";
                learnerViewModel.Person.DateCreated = DateTime.Now; 

                learnerViewModel.Person.LastUpdatedBy = "admin";
                learnerViewModel.Person.DateUpdated = DateTime.Now;
                
                learnerViewModel.Address.CreatedBy = "admin";
                learnerViewModel.Address.DateCreated = DateTime.Now;

                learnerViewModel.Address.LastUpdatedBy = "admin";
                learnerViewModel.Address.DateUpdated = DateTime.Now;
                            
                learnerViewModel.Learner.CreatedBy = "admin";
                learnerViewModel.Learner.DateCreated = DateTime.Now;
 
                learnerViewModel.Learner.LastUpdatedBy = "admin";
                learnerViewModel.Address.DateUpdated = DateTime.Now;
                
                learnerViewModel.Learner.AppliedYn = Const.FALSE;
                learnerViewModel.Learner.RecruitedYn = Const.FALSE;
                
                //Link the Address to the Person     
                learnerViewModel.Person.Address.Add(learnerViewModel.Address); 

                // Add the Person into the Database
                var user = await _lookUpService.GetUserByUsrname(User.Identity.Name);

                learnerViewModel.Person.UserId = user.Id;
                learnerViewModel.Person.Email = user.Email;
                
                //Now link the learner to this Person Profile created 
                learnerViewModel.Learner.Person = learnerViewModel.Person;

                //Then Add the Company

                //Then Add the Learner to the Database     
                _context.Add(learnerViewModel.Learner);
 
                //Save the Person and Address in to the Database
                await _context.SaveChangesAsync();
                _notyf.Warning("Profile created successful...", 5);
                
                ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc",learnerViewModel.Person.CitizenshipStatusId);
                ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc",learnerViewModel.Person.DisabilityStatusId);
                ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc",learnerViewModel.Person.EquityId);
                ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc",learnerViewModel.Person.GenderId);
                ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc",learnerViewModel.Person.HomeLanguageId);
                ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc",learnerViewModel.Person.NationalityId);
                ViewData["SuburbId"] = new SelectList(_lookUpService.GetSuburbs().Result, "id", "name",learnerViewModel.Address.SuburbId);
                ViewData["CityId"] = new SelectList(await _lookUpService.GetCities(), "id", "name",learnerViewModel.Address.CityId);
                ViewData["ProvinceId"] = new SelectList(_lookUpService.GetProvinces().Result, "id", "name",learnerViewModel.Address.ProvinceId);
                ViewData["CountryId"] = new SelectList(_lookUpService.GetCountries().Result, "id", "name",learnerViewModel.Address.CountryId);
                ViewData["AddressTypeId"] = new SelectList(_lookUpService.GetAddressTypes().Result, "id", "name",learnerViewModel.Person.CitizenshipStatusId);
                /*ViewData["InstitutionId"] = new SelectList(_lookUpService.GetInstitutions().Result, "id", "name",learnerViewModel.Learner.);
                ViewData["CourseId"] = new SelectList(_lookUpService.GetCourses().Result, "id", "name",learnerViewModel.Person.CitizenshipStatusId);*/
                ViewData["SchoolId"] = new SelectList(_lookUpService.GetSchools().Result, "id", "name",learnerViewModel.Learner.SchoolId);
                ViewData["SchoolGradeId"] = new SelectList(_lookUpService.GetSchoolGrades().Result, "id", "name",learnerViewModel.Learner.SchoolGradeId); 
                return RedirectToAction(nameof(Details), new { id = learnerViewModel.Person.NationalId });
            }

            return View();
        }

        // GET: Person/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _lookUpService.GetPersonByNationalIdForEditDelete(id);
            var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
            var userRole =  _userManager.GetRolesAsync(user).Result;
            
            if (learner == null )
            {
                return NotFound();
            } 

            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc");
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc");
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc");
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc");
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc");
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc");
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "UserId");
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName");
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbName");
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName");
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeName");
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolName");
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeName");
            ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
            return View(learner);
        }
   
        // POST: Person/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id,[Bind("LearnerId,PersonId,SchoolId,SchoolGradeId,MotivationText,YearSchoolCompleted,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated,RecruitedYn,AppliedYn,Person,School,SchoolGrade")] Learner learner)
        {
            if (learner.Person == null )
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                learner.Person.LastUpdatedBy = "admin";
                learner.Person.DateUpdated = DateTime.Now;
                
               //  lnr.LearnerCourse.AddRange(lnr.LearnerCourse);
              try
                {
                     _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonExists(learner.Person.NationalId) && !AddressExists(learner.Person.Address.FirstOrDefault().AddressId))
                    {
                        return NotFound();
                    } 
                    else
                    {
                        throw;
                    }
                }
              _notyf.Success("Your details were edited successfully...");
                return RedirectToAction(nameof(Index));
            }
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc", learner.Person.CitizenshipStatusId);
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc", learner.Person.DisabilityStatusId);
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc", learner.Person.EquityId);
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc", learner.Person.GenderId);
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc", learner.Person.HomeLanguageId);
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc", learner.Person.NationalityId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", learner.Person.Address.FirstOrDefault().CityId);
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbName", learner.Person.Address.FirstOrDefault().SuburbId);
            ViewData["CourseId"] = new SelectList(_context.Course, "CourseId", "CourseTitle");  
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", learner.Person.Address.FirstOrDefault().CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", learner.Person.Address.FirstOrDefault().ProvinceId);
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeName", learner.Person.Address.FirstOrDefault().AddressTypeId);
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolName", learner.SchoolId);
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeName", learner.SchoolGradeId);
            return View(learner);
        }

        // GET: Person/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var learner = await _lookUpService.GetPersonByNationalIdForEditDelete(id);
            
            if (learner.Person.NationalId == null)
            {
                return NotFound();
            }

            return View(learner.Person);
        }
 
        // POST: Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        { 
            // This block deletes an entry's dependencies in a database.
            var addresses = new List<Address>();
            addresses = (from address in _context.Address where address.PersonId.Equals(id) select address).ToList();
 
            foreach (var address  in addresses ) 
            {
                _context.Address.Remove( address); //execution from DB
            }
             
            var learner = _context.Learner.Include(lc => lc.LearnerCourse).Where(m => m.Person.NationalId.Equals(id)).FirstOrDefault();

            if (learner != null)
            {
                foreach (var lc  in learner.LearnerCourse )
                {  
                    if(lc.LearnerCourseId != 0)
                    _context.LearnerCourse.Remove( lc); //execution from DB
                }
                 
                _context.Remove(learner);
            }
            
            var person = _context.Person.Where(m => m.NationalId.Equals(id)).FirstOrDefault();
            _context.Person.Remove(person); 
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonExists(string id)
        {
            return _context.Person.Any(e => e.NationalId.Equals(id));
        }
        
        private bool AddressExists(long id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }
        
    }
}
