using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using learner_portal.DTO;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{
    [Authorize]
    public class SchoolController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public SchoolController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: School
        public async Task<IActionResult> Index()
        {
            return View(await _context.School.ToListAsync());
        }

        public async Task<JsonResult> GetAllSchool()
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

                var listOfSchool = await _lookUpService.GetSchoolDetails();

                // Getting all Customer data  z
                var allSchool = listOfSchool;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allSchool = allSchool.Where(m =>
                            m.SchoolCode == searchValue ||
                            m.SchoolName == searchValue)
                        as List<SchoolDTO>;
                }

                //total number of rows count   
                recordsTotal = allSchool.Count();
                //Paging   
                var dataList = allSchool.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allSchool });
            }
            catch (Exception)
            {
                throw;
            } 
        } 
        
        // GET: School/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var schoolDetails =  await _lookUpService.GetSchoolDetailsById(id);
            if (schoolDetails == null)
            {
                return NotFound();
            }

            return PartialView(schoolDetails);
        }

        // GET: School/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: School/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolId,SchoolName,SchoolCode,EmisNo")] School school)
        {
            if (ModelState.IsValid)
            {
                _context.Add(school);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(school);
        }

        // GET: School/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var school =  await _lookUpService.GetSchoolByIdForEditDelete(id);

            if (school == null)
            {
                return NotFound();
            }
            return PartialView(school);
        }

        // POST: School/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SchoolId,SchoolName,SchoolCode,EmisNo")] School school)
        {
            if (id != school.SchoolId)
            { 
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(school);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolExists(school.SchoolId))
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
            return PartialView(school);
        }   
  
        // GET: School/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var school = await _lookUpService.GetSchoolByIdForEditDelete(id);
            if (school.SchoolId == null)
            {
                return NotFound();
            }

            return PartialView(school);
        }

        // POST: School/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var school = await _context.School.FindAsync(id);
            _context.School.Remove(school);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolExists(long id)
        {
            return _context.School.Any(e => e.SchoolId.Equals(id));
        }
    }
}
