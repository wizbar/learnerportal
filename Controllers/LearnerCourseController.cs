using System;
using System.Collections.Generic;
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
using Enum = learner_portal.Helpers.Enum;

namespace learner_portal.Controllers
{
    [Authorize]
    public class LearnerCourseController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;
        private readonly INotyfService _notyf;
        public LearnerCourseController(LearnerContext context,ILookUpService lookUpService,INotyfService notyf)
        {
            _context = context;
            _lookUpService = lookUpService;
            _notyf = notyf;
        }
 
        // GET: LearnerCourse
        public async Task<IActionResult> Index()
        {
            return View(await _context.LearnerCourse.ToListAsync());
        }

        // GET: LearnerCourse/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerCourse = await _context.LearnerCourse
                .FirstOrDefaultAsync(m => m.LearnerCourseId == id);
            if (learnerCourse == null)
            {
                return NotFound();
            }

            return View(learnerCourse);
        }

        // GET: LearnerCourse/Create
        public IActionResult Create()
        {  
            return View(); 
        }

        // POST: LearnerCourse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LearnerCourseId,LearnerId,InstitutionName,CourseName,DateOfCompletion,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] LearnerCourse learnerCourse)
        {
            //Get current user details  
            var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
            
            //Get current leaner details
            var learner = await _lookUpService.GetLearnerDetailsByIdEmail(user.Email);
            
            if (ModelState.IsValid)
            {
             
                //assign Leaner Id to link these qualifications to leaner 
                learnerCourse.LearnerId = learner.LearnerId;
             
                //create an audit trail
                learnerCourse.CreatedBy = user.UserName;
                learnerCourse.DateCreated = DateTime.Now; 

                learnerCourse.LastUpdatedBy =  user.UserName;
                learnerCourse.DateUpdated = DateTime.Now;
                  
                _context.Add(learnerCourse);
                await _context.SaveChangesAsync();
                
                return RedirectToAction("Details","Person", new {id= learner.NationalID});
            }
            
            return RedirectToAction("Details","Person", new {id= learner.NationalID});
        }

        
        // GET: LearnerCourse/Create
        public IActionResult _AddQualification()
        {
          return PartialView(); 
        }
 
        // POST: LearnerCourse/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddQualification(LearnerCourse learnerCourse)
        {
            //Get current user details 
            var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
            
            //Get currect leaner details
             var learner = await _lookUpService.GetLearnerDetailsByIdEmail(user.Person.NationalId);
             
             //assign Leaner Id to link these qualifications to leaner 
             learnerCourse.LearnerId = learner.LearnerId;
             
             //create an audit trail
            learnerCourse.CreatedBy = user.UserName;
            learnerCourse.DateCreated = DateTime.Now;

            if (ModelState.IsValid)
            {  
                _context.Add(learnerCourse);
                await _context.SaveChangesAsync();
                _notyf.Success("Qualification added successfully...");
                return RedirectToAction(nameof(Index)); 
            }
            return PartialView(learnerCourse);
        }

        // GET: LearnerCourse/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerCourse = await _context.LearnerCourse.Where(a => a.LearnerCourseId == id).Include(a => a.Learner).ThenInclude(a => a.Person).FirstOrDefaultAsync();
            if (learnerCourse == null)
            { 
                return NotFound();
            }
            return PartialView(learnerCourse);
        }

        // POST: LearnerCourse/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("LearnerCourseId,LearnerId,InstitutionName,CourseName,DateOfCompletion,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] LearnerCourse learnerCourse)
        {
            if (id != learnerCourse.LearnerCourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var person = await _lookUpService.GetLearnerDetailsById(learnerCourse.LearnerId);
                
                learnerCourse.LastUpdatedBy = User.Identity.Name; 
                learnerCourse.DateUpdated = DateTime.Now;
                
                try 
                {
                    _context.Update(learnerCourse);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LearnerCourseExists(learnerCourse.LearnerCourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Success("Qualification edited successfully...");
                return RedirectToAction("Details","Person", new { Id = person.NationalID});
            }
            return View();
        }

        // GET: LearnerCourse/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var learnerCourse = await _context.LearnerCourse
                .FirstOrDefaultAsync(m => m.LearnerCourseId == id);
            if (learnerCourse == null)
            {
                return NotFound();
            }
  
            return View(learnerCourse);
        }

        // POST: LearnerCourse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var learnerCourse = await _context.LearnerCourse.FindAsync(id);
            _context.LearnerCourse.Remove(learnerCourse);
            await _context.SaveChangesAsync();
            _notyf.Success("Qualification deleted successfully...");
            var person = await _lookUpService.GetLearnerDetailsById(learnerCourse.LearnerId);
            return RedirectToAction("Details","Person", new { Id = person.NationalID});
        }

        private bool LearnerCourseExists(long id)
        {
            return _context.LearnerCourse.Any(e => e.LearnerCourseId == id);
        }
    }
}
