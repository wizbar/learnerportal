using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{
    [Authorize]
    public class SchoolGradeController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public SchoolGradeController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: SchoolGrade
        public async Task<IActionResult> Index()
        {
            return View(await _context.SchoolGrade.ToListAsync());
        }
        
        public JsonResult GetAllSchoolGrade()
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

                var listOfSchoolGrade = new List<SchoolGrade>();

                listOfSchoolGrade = _lookUpService.GetAllSchoolGrade().Result;

                // Getting all Customer data  z
                var allSchoolGrade = listOfSchoolGrade;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allSchoolGrade = allSchoolGrade.Where(m =>
                            m.SchoolGradeCode == searchValue ||
                            m.SchoolGradeName == searchValue)
                        as List<SchoolGrade>;
                }

                //total number of rows count   
                recordsTotal = allSchoolGrade.Count();
                //Paging   
                var dataList = allSchoolGrade.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allSchoolGrade });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: SchoolGrade/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolGrade = await _context.SchoolGrade
                .FirstOrDefaultAsync(m => m.SchoolGradeId == id);
            if (schoolGrade == null)
            {
                return NotFound();
            }

            return PartialView(schoolGrade);
        }

        // GET: SchoolGrade/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: SchoolGrade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchoolGradeId,SchoolGradeCode,SchoolGradeName")] SchoolGrade schoolGrade)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolGrade);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(schoolGrade);
        }

        // GET: SchoolGrade/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolGrade = await _context.SchoolGrade.FindAsync(id);
            if (schoolGrade == null)
            {
                return NotFound();
            }
            return PartialView(schoolGrade);
        }

        // POST: SchoolGrade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SchoolGradeId,SchoolGradeCode,SchoolGradeName")] SchoolGrade schoolGrade)
        {
            if (id != schoolGrade.SchoolGradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolGrade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolGradeExists(schoolGrade.SchoolGradeId))
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
            return PartialView(schoolGrade);
        }

        // GET: SchoolGrade/Delete/5
        public async Task<IActionResult> Delete(long? id) 
        { 
            if (id == null) 
            {
                return NotFound();
            }

            var schoolGrade = await _context.SchoolGrade
                .FirstOrDefaultAsync(m => m.SchoolGradeId == id);
            if (schoolGrade == null)
            {
                return NotFound();
            }

            return PartialView(schoolGrade);
        }

        // POST: SchoolGrade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var schoolGrade = await _context.SchoolGrade.FindAsync(id);
            _context.SchoolGrade.Remove(schoolGrade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolGradeExists(long id)
        {
            return _context.SchoolGrade.Any(e => e.SchoolGradeId == id);
        }
    }
}
