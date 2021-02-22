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
    public class InstitutionTypeController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public InstitutionTypeController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService; 
        }

        // GET: InstitutionType
        public async Task<IActionResult> Index()
        {
            return View(await _context.InstitutionType.ToListAsync());
        }
        
        public JsonResult GetAllInstitutionType()
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

                var listOfInstitutionType = new List<InstitutionType>();
                 
                listOfInstitutionType = _lookUpService.GetAllInstitutionType().Result;

                // Getting all Customer data  z
                var allInstitutionType = listOfInstitutionType;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allInstitutionType = allInstitutionType.Where(m =>
                            m.InstitutionTypeCode == searchValue ||
                            m.InstitutionTypeDesc == searchValue)
                        as List<InstitutionType>;
                }

                //total number of rows count   
                recordsTotal = allInstitutionType.Count();
                //Paging   
                var dataList = allInstitutionType.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allInstitutionType });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: InstitutionType/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionType = await _context.InstitutionType
                .FirstOrDefaultAsync(m => m.InstitutionTypeId == id);
            if (institutionType == null)
            {
                return NotFound();
            }

            return PartialView(institutionType);
        }

        // GET: InstitutionType/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: InstitutionType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutionTypeId,InstitutionTypeDesc,InstitutionTypeCode")] InstitutionType institutionType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institutionType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(institutionType);
        }

        // GET: InstitutionType/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionType = await _context.InstitutionType.FindAsync(id);
            if (institutionType == null)
            {
                return NotFound();
            }
            return PartialView(institutionType); 
        }

        // POST: InstitutionType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("InstitutionTypeId,InstitutionTypeDesc,InstitutionTypeCode")] InstitutionType institutionType)
        {
            if (id != institutionType.InstitutionTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institutionType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionTypeExists(institutionType.InstitutionTypeId))
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
            return PartialView(institutionType);
        }

        // GET: InstitutionType/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var institutionType = await _context.InstitutionType
                .FirstOrDefaultAsync(m => m.InstitutionTypeId == id);
            if (institutionType == null)
            {
                return NotFound();
            }

            return PartialView(institutionType);
        }

        // POST: InstitutionType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var institutionType = await _context.InstitutionType.FindAsync(id);
            _context.InstitutionType.Remove(institutionType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        } 

        private bool InstitutionTypeExists(long id)
        {
            return _context.InstitutionType.Any(e => e.InstitutionTypeId == id);
        }
    }
}
