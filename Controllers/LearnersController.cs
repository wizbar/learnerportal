using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learner_portal.DTO;
using learner_portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{
    [Authorize]
    public class LearnersController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public LearnersController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Learners
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Learner.Include(l => l.Person).Include(l => l.School).Include(l => l.SchoolGrade);
            return View(await learnerContext.ToListAsync());
        }
 
        public async Task<JsonResult> GetAllLearners()
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

               var listOfLearners = await _lookUpService.GetAllLearners().ConfigureAwait(false);
               
               listOfLearners = listOfLearners.Where(l => !l.AppliedYn.Equals(Const.TRUE)).ToList();
                // Getting all Customer data  z
                var allLearners = listOfLearners.Select(a => new LearnerDetailsDto()
                {
                            LearnerId = a.LearnerId,
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
                            HouseNo = a.Person.Address.ToList()[0].HouseNo,
                            StreetName = a.Person.Address.ToList()[0].StreetName,
                            SuburbName = a.Person.Address.ToList()[0].Suburb.SuburbName,
                            CityName = a.Person.Address.ToList()[0].City.CityName,
                            ProvinceName = a.Person.Address.ToList()[0].Province.ProvinceName,
                            CountryName = a.Person.Address.ToList()[0].Country.CountryName,
                            AddressType = a.Person.Address.ToList()[0].AddressType.AddressTypeName,
                            PostalCode = a.Person.Address.ToList()[0].PostalCode,
                            SchoolName = a.School.SchoolName,
                            SchoolGradeName = a.SchoolGrade.SchoolGradeName,
                            YearSchoolCompleted = a.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                            RecruitedYn = a.RecruitedYn,
                            AppliedYn = a.AppliedYn,
                            PhotoName = a.Person.PhotoName,
                            PhotoPath = a.Person.PhotoPath,
                            Qualifications = a.LearnerCourse.Select(qualification =>
                                new QualificationDTO()
                                {
                                    Id = qualification.LearnerCourseId,
                                    CourseName = qualification.CourseName,
                                    InstitutionName = qualification.InstitutionName,
                                    DateOfCompletion = qualification.DateOfCompletion
                                }) as List<QualificationDTO>,
                }).ToList();
                 
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allLearners = allLearners.Where(m =>
                            m.FirstName.Equals(searchValue) ||
                            m.LastName.Equals(searchValue) ||
                            m.NationalID.Equals(searchValue))
                        as List<LearnerDetailsDto>;
                }

                //total number of rows count   
                recordsTotal = allLearners.Count();
                //Paging   
                var dataList = allLearners.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allLearners });
            }
            catch (Exception)
            {
                throw;
            }
        }       
        
        public async Task<JsonResult> GetAllAppliedLearners()
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

               var listOfLearners = await _lookUpService.GetAllLearners();
               
               listOfLearners = listOfLearners.Where(l => l.AppliedYn.Equals(Const.TRUE) && l.RecruitedYn.Equals(Const.FALSE)).ToList();
                // Getting all Customer data  z
                var allLearners = listOfLearners.Select(a => new LearnerDetailsDto()
                {
                            LearnerId = a.LearnerId,
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
                            HouseNo = a.Person.Address.ToList()[0].HouseNo,
                            StreetName = a.Person.Address.ToList()[0].StreetName,
                            SuburbName = a.Person.Address.ToList()[0].Suburb.SuburbName,
                            CityName = a.Person.Address.ToList()[0].City.CityName,
                            ProvinceName = a.Person.Address.ToList()[0].Province.ProvinceName,
                            CountryName = a.Person.Address.ToList()[0].Country.CountryName,
                            AddressType = a.Person.Address.ToList()[0].AddressType.AddressTypeName,
                            PostalCode = a.Person.Address.ToList()[0].PostalCode,
                            SchoolName = a.School.SchoolName,
                            SchoolGradeName = a.SchoolGrade.SchoolGradeName,
                            YearSchoolCompleted = a.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                            RecruitedYn = a.RecruitedYn,
                            AppliedYn = a.AppliedYn,
                            PhotoName = a.Person.PhotoName,
                            PhotoPath = a.Person.PhotoPath,
                            Qualifications = a.LearnerCourse.Select(qualification =>
                                new QualificationDTO()
                                {  
                                    Id = qualification.LearnerCourseId,
                                    CourseName = qualification.CourseName,
                                    InstitutionName = qualification.InstitutionName,
                                    DateOfCompletion = qualification.DateOfCompletion
                                }) as List<QualificationDTO>,
                }).ToList();
                 
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allLearners = allLearners.Where(m =>
                            m.FirstName.Equals(searchValue) ||
                            m.LastName.Equals(searchValue) ||
                            m.NationalID.Equals(searchValue))
                        as List<LearnerDetailsDto>;
                }

                //total number of rows count   
                recordsTotal = allLearners.Count();
                //Paging   
                var dataList = allLearners.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allLearners });
            }
            catch (Exception)
            {
                throw;
            }
        }       public async Task<JsonResult> GetAllRecruitedLearners()
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

               var listOfLearners = await _lookUpService.GetAllLearners();
               
               listOfLearners = listOfLearners.Where(l => l.RecruitedYn.Equals(Const.TRUE) && l.AppliedYn.Equals(Const.TRUE) ).ToList();
                // Getting all Customer data  z
                var allLearners = listOfLearners.Select(a => new LearnerDetailsDto()
                {
                            LearnerId = a.LearnerId,
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
                            HouseNo = a.Person.Address.ToList()[0].HouseNo,
                            StreetName = a.Person.Address.ToList()[0].StreetName,
                            SuburbName = a.Person.Address.ToList()[0].Suburb.SuburbName,
                            CityName = a.Person.Address.ToList()[0].City.CityName,
                            ProvinceName = a.Person.Address.ToList()[0].Province.ProvinceName,
                            CountryName = a.Person.Address.ToList()[0].Country.CountryName,
                            AddressType = a.Person.Address.ToList()[0].AddressType.AddressTypeName,
                            PostalCode = a.Person.Address.ToList()[0].PostalCode,
                            SchoolName = a.School.SchoolName,
                            SchoolGradeName = a.SchoolGrade.SchoolGradeName,
                            YearSchoolCompleted = a.YearSchoolCompleted.ToString(Const.DATE_FORMAT),
                            RecruitedYn = a.RecruitedYn,
                            AppliedYn = a.AppliedYn,
                            PhotoName = a.Person.PhotoName, 
                            PhotoPath = a.Person.PhotoPath, 
                            Qualifications = a.LearnerCourse.Select(qualification =>
                                new QualificationDTO()
                                {
                                    Id = qualification.LearnerCourseId,
                                    CourseName = qualification.CourseName,
                                    InstitutionName = qualification.InstitutionName,
                                    DateOfCompletion = qualification.DateOfCompletion
                                }) as List<QualificationDTO>,
                }).ToList();
                 
                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allLearners = allLearners.Where(m =>
                            m.FirstName.Equals(searchValue) ||
                            m.LastName.Equals(searchValue) ||
                            m.NationalID.Equals(searchValue))
                        as List<LearnerDetailsDto>;
                }

                //total number of rows count   
                recordsTotal = allLearners.Count();
                //Paging   
                var dataList = allLearners.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allLearners });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Learners/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var learner = await _lookUpService.GetLearnerDetailsById(id);
            if (learner == null)
            {
                return NotFound();
            }

            return PartialView(learner);
        }

        // GET: Learners/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId");
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolId");
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeId");
            return PartialView();
        }

        // POST: Learners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerId,PersonId,SchoolId,SchoolGradeId,MotivationText,YearSchoolCompleted,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                
                learner.CreatedBy = "admin";
                learner.DateCreated = DateTime.Now;

                learner.LastUpdatedBy = "admin";
                learner.DateUpdated = DateTime.Now;
                
                _context.Add(learner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", learner.PersonId);
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolId", learner.SchoolId);
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeId", learner.SchoolGradeId);
            return PartialView(learner);
        }

        // GET: Learners/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learner.FindAsync(id);
            if (learner == null)
            {
                return NotFound();
            }

            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", learner.PersonId);
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolId", learner.SchoolId);
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeId", learner.SchoolGradeId);
            return PartialView(learner);
        }

        // POST: Learners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598. 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("LearnerId,PersonId,SchoolId,SchoolGradeId,MotivationText,YearSchoolCompleted,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Learner learner)
        {
            if (id != learner.LearnerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                learner.LastUpdatedBy = "admin";
                learner.DateUpdated = DateTime.Now;
                
                try
                {
                    _context.Update(learner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerExists(learner.LearnerId))
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

            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", learner.PersonId);
            ViewData["SchoolId"] = new SelectList(_context.School, "SchoolId", "SchoolId", learner.SchoolId);
            ViewData["SchoolGradeId"] = new SelectList(_context.SchoolGrade, "SchoolGradeId", "SchoolGradeId", learner.SchoolGradeId);
            return PartialView(learner);
        }

        // GET: Learners/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learner = await _context.Learner
                .Include(l => l.Person)
                .Include(l => l.School)
                .Include(l => l.SchoolGrade)
                .FirstOrDefaultAsync(m => m.LearnerId == id);
            if (learner == null)
            {
                return NotFound();
            }

            return PartialView(learner);
        }

        // POST: Learners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var learner = await _context.Learner.FindAsync(id);
            _context.Learner.Remove(learner);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LearnerExists(long id)
        {
            return _context.Learner.Any(e => e.LearnerId == id);
        }
    }
}
