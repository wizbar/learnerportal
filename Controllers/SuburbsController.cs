using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learner_portal.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;

namespace learner_portal.Controllers
{
    public class SuburbsController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public SuburbsController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Suburbs
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Suburb.Include(s => s.City);
            return View(await learnerContext.ToListAsync());
        }
        
         public async Task<JsonResult> GetAllSuburbs()
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
 
                
               var listOfSuburbs = await _lookUpService.GetSuburbsDetails();

                // Getting all Customer data  z 
                var allSuburbs = listOfSuburbs;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allSuburbs = allSuburbs.Where(m =>
                            m.SuburbCode == searchValue ||
                            m.SuburbName == searchValue)
                        as List<SuburbsDetailsDTO>;  
                }

                //total number of rows count   
                recordsTotal = allSuburbs.Count();
                //Paging   
                var data = allSuburbs.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allSuburbs });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Suburbs/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var ProvincesDetails = await  _lookUpService.GetAllSuburbsById(id);
            
            if (ProvincesDetails == null)
            {
                return NotFound();
            }

            return PartialView(ProvincesDetails);
        }

        // GET: Suburbs/Create
        public IActionResult Create()
        {
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName");
            return PartialView();
        }

        // POST: Suburbs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SuburbId,SuburbName,SuburbCode,CityId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Suburb suburb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suburb);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", suburb.CityId);
            return PartialView(suburb);
        }

        // GET: Suburbs/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var suburb = await _lookUpService.GetSuburbsDetailsByIdForEditDelete(id);
            
            if (suburb == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", suburb.CityId);
            return PartialView(suburb);
        }

        // POST: Suburbs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SuburbId,SuburbName,SuburbCode,CityId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Suburb suburb)
        {
            if (id != suburb.SuburbId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suburb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuburbExists(suburb.SuburbId))
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
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", suburb.CityId);
            return PartialView(suburb);
        }

        // GET: Suburbs/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var suburb = await _lookUpService.GetSuburbsDetailsByIdForEditDelete(id);
            
            if (suburb == null)
            {
                return NotFound();
            }

            return PartialView(suburb);
        }

        // POST: Suburbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var suburb = await _context.Suburb.FindAsync(id);
            _context.Suburb.Remove(suburb);
            await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(Index));
        }

        private bool SuburbExists(long id)
        {
            return _context.Suburb.Any(e => e.SuburbId.Equals(id));
        }
    }
}
