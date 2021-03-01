using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using learner_portal.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Identity;

namespace learner_portal.Controllers
{
    public class AssessorsController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;
        private readonly UserManager<Users> _userManager;
        private readonly INotyfService _notyf;

        public AssessorsController(LearnerContext context, ILookUpService lookUpService, UserManager<Users> userManager, INotyfService notyf)
        {
            _context = context;
            _lookUpService = lookUpService;
            _userManager = userManager;
            _notyf = notyf;
        }

        // GET: Assessors
        public async Task<IActionResult> Index()
        {

            return View();
        }
        
        public async Task<JsonResult> GetAllAssessors()
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

                
               var listOfAssessors = await _lookUpService.GetAssessorDetails().ConfigureAwait(false);

                // Getting all Customer data  z 
                var allAssessors = listOfAssessors;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allAssessors = allAssessors.Where(m =>
                            m.ApplicationTypesDesc == searchValue ||
                            m.CertificateIssuedYn == searchValue)
                        as List<AssessorDetailsDTO>;
                }

                //total number of rows count   
                recordsTotal = allAssessors.Count();
                //Paging   
                var data = allAssessors.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allAssessors });
            }
            catch (Exception)
            {
                throw;
            }  
        }

        // GET: Assessors/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessor = await _context.Assessor
                .Include(a => a.AccreditationStatuses)
                .Include(a => a.Etqe)
                .Include(a => a.ProcessIndicators)
                .FirstOrDefaultAsync(m => m.AssessorId == id);
            if (assessor == null)
            {
                return NotFound();
            }

            return View(assessor);
        }

        // GET: Assessors/Create
        public async Task<IActionResult> Create()
        {
            ViewData["AccreditationStatusId"] = new SelectList(_context.AccreditationStatuses, "AccreditationStatusId", "AccreditationStatusDesc");
            ViewData["EtqeId"] = new SelectList(_context.Etqe, "EtqeId", "EtqeName");
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc");
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc");
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc");
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc");
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc");
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc");
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationId", "QualificationTitle");
            ViewData["CityId"] = new SelectList(await _lookUpService.GetCities(), "id", "name");
            ViewData["SuburbId"] = new SelectList(_lookUpService.GetSuburbs().Result, "id", "name");
            ViewData["CountryId"] = new SelectList(_lookUpService.GetCountries().Result, "id", "name");
            ViewData["ProvinceId"] = new SelectList(_lookUpService.GetProvinces().Result, "id", "name");
            ViewData["AddressTypeId"] = new SelectList(_lookUpService.GetAddressTypes().Result, "id", "name");
            ViewData["SchoolId"] = new SelectList(_lookUpService.GetSchools().Result, "id", "name");
            ViewData["SchoolGradeId"] = new SelectList(_lookUpService.GetSchoolGrades().Result, "id", "name");
            return View();
        }

        // POST: Assessors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Person", "Address", "Assessor")] AssessorViewModel assessorViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
                
                assessorViewModel.Assessor.CreatedBy = user.UserName;
                assessorViewModel.Assessor.DateCreated = DateTime.Now;
                assessorViewModel.Assessor.ApplicationDate = DateTime.Now;
                
                //Link the Address to the Person
                assessorViewModel.Person.Address.Add(assessorViewModel.Address); 
                
                // Add the Person into the Database
                var member = await _lookUpService.GetUserByUsrname(User.Identity.Name);

                assessorViewModel.Person.UserId = member.Id;
                assessorViewModel.Person.Email = member.Email;

                //Now link the Assessor to this Person Profile created 
                assessorViewModel.Assessor.Person = assessorViewModel.Person;
                
                //Now link the learner to this Person Profile created 
                assessorViewModel.Learner.Person = assessorViewModel.Person;
                
                //Then Add the Learner to the Database     
                _context.Add(assessorViewModel.Learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccreditationStatusId"] = new SelectList(_context.AccreditationStatuses, "AccreditationStatusId", "AccreditationStatusDesc", assessorViewModel.Assessor.AccreditationStatusesId);
            ViewData["EtqeId"] = new SelectList(_context.Etqe, "EtqeId", "EtqeName", assessorViewModel.Assessor.EtqeId);
            ViewData["ApplicationTypesId"] = new SelectList(_context.ApplicationType, "ApplicationTypesId", "ApplicationTypesDesc", assessorViewModel.Assessor.ApplicationTypes);
            ViewData["EvaluatorsId"] = new SelectList(_context.Evaluator, "EvaluatorsId", "EvaluatorsId", assessorViewModel.Assessor.Evaluators);
            ViewData["CitizenshipStatusId"] = new SelectList(_context.CitizenshipStatus, "CitizenshipStatusId", "CitizenshipStatusDesc", assessorViewModel.Person.CitizenshipStatus);
            ViewData["DisabilityStatusId"] = new SelectList(_context.DisabilityStatus, "DisabilityStatusId", "DisabilityStatusDesc", assessorViewModel.Person.DisabilityStatus);
            ViewData["EquityId"] = new SelectList(_context.Equity, "EquityId", "EquityDesc", assessorViewModel.Person.Equity);
            ViewData["GenderId"] = new SelectList(_context.Gender, "GenderId", "GenderDesc", assessorViewModel.Person.Gender);
            ViewData["HomeLanguageId"] = new SelectList(_context.HomeLanguage, "HomeLanguageId", "HomeLanguageDesc", assessorViewModel.Person.HomeLanguage);
            ViewData["QualificationId"] = new SelectList(_context.Qualification, "QualificationId", "QualificationTitle", assessorViewModel.Qualification.QualificationId);
            ViewData["NationalityId"] = new SelectList(_context.Nationality, "NationalityId", "NationalityDesc", assessorViewModel.Person.Nationality);
            ViewData["CityId"] = new SelectList(await _lookUpService.GetCities(), "id", "name", assessorViewModel.Address.City);
            ViewData["SuburbId"] = new SelectList(_lookUpService.GetSuburbs().Result, "id", "name", assessorViewModel.Address.Suburb);
            ViewData["CountryId"] = new SelectList(_lookUpService.GetCountries().Result, "id", "name", assessorViewModel.Address.Country);
            ViewData["ProvinceId"] = new SelectList(_lookUpService.GetProvinces().Result, "id", "name", assessorViewModel.Address.Province);
            ViewData["AddressTypeId"] = new SelectList(_lookUpService.GetAddressTypes().Result, "id", "name", assessorViewModel.Address.AddressType);
            ViewData["SchoolId"] = new SelectList(_lookUpService.GetSchools().Result, "id", "name", assessorViewModel.Learner.School);
            ViewData["SchoolGradeId"] = new SelectList(_lookUpService.GetSchoolGrades().Result, "id", "name", assessorViewModel.Learner.SchoolGrade);
            return View(assessorViewModel);
        }

        // GET: Assessors/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessor = await _context.Assessor.FindAsync(id);
            if (assessor == null)
            {
                return NotFound();
            }
            ViewData["AccreditationStatusId"] = new SelectList(_context.AccreditationStatuses, "AccreditationStatusId", "AccreditationStatusId", assessor.AccreditationStatusesId);
            ViewData["EtqeId"] = new SelectList(_context.Etqe, "EtqeId", "EtqeId", assessor.EtqeId);
            return View(assessor);
        }

        // POST: Assessors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AssessorId,PersonId,AccredStartDate,AccredEndDate,AccreditationStatusId,RegistrationNo,EvaluatorsId,ApplicationDate,RegistrationDate,ApprovedBy,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated,ProcessIndicatorsId,ApplicationTypesId,SendForApprovalDate,CertificateIssuedYn,CertificateDate,CertificateNo,ReissueDate,EtqeId")] Assessor assessor)
        {
            if (id != assessor.AssessorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
                
                assessor.LastUpdatedBy = user.UserName;
                assessor.DateUpdated = DateTime.Now;
                
                try
                {
                    _context.Update(assessor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AssessorExists(assessor.AssessorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AccreditationStatusId"] = new SelectList(_context.AccreditationStatuses, "AccreditationStatusId", "AccreditationStatusId", assessor.AccreditationStatusesId);
            ViewData["EtqeId"] = new SelectList(_context.Etqe, "EtqeId", "EtqeId", assessor.EtqeId);
            return View(assessor);
        }

        // GET: Assessors/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assessor = await _context.Assessor
                .Include(a => a.AccreditationStatuses)
                .Include(a => a.Etqe)
                .Include(a => a.ProcessIndicators)
                .FirstOrDefaultAsync(m => m.AssessorId == id);
            if (assessor == null)
            {
                return NotFound();
            }

            return View(assessor);
        }

        // POST: Assessors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var assessor = await _context.Assessor.FindAsync(id);
            _context.Assessor.Remove(assessor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssessorExists(long id)
        {
            return _context.Assessor.Any(e => e.AssessorId == id);
        }
    }
}
