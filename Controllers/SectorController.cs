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
    public class SectorController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService; 

        public SectorController(LearnerContext context, ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Sector
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sector.ToListAsync());
        }


        public async Task<JsonResult> GetAllSector()
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

            var listOfSector = new List<Sector>();

            listOfSector = await _lookUpService.GetAllSector();

            // Getting all Customer data  z
            var allsector = listOfSector;

            //Search
            if (!string.IsNullOrEmpty(searchValue))
            {
                allsector = allsector.Where(m =>
                        m.SectorDesc == searchValue ||
                        m.ActiveYn == searchValue)
                    as List<Sector>;
            }

            //total number of rows count   
            recordsTotal = allsector.Count();
            //Paging   
            var dataList = allsector.Skip(skip).Take(pageSize).ToList();
            //Returning Json Data  
            return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allsector });
        }

        // GET: Sector/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sector = await _context.Sector
                .FirstOrDefaultAsync(m => m.SectorId == id);
            if (sector == null)
            {
                return NotFound();
            }

            return PartialView(sector);
        }

        // GET: Sector/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Sector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SectorId,SectorDesc,ActiveYn")] Sector sector)
        {
            if (!ModelState.IsValid) return PartialView(sector);
            _context.Add(sector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Sector/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sector = await _context.Sector.FindAsync(id);
            if (sector == null) 
            {
                return NotFound();
            } 
            return PartialView(sector);
        }

        // POST: Sector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("SectorId,SectorDesc,ActiveYn")] Sector sector)
        {
            if (id != sector.SectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SectorExists(sector.SectorId))
                    {
                        return NotFound();
                    }

                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return PartialView(sector);
        }

        // GET: Sector/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sector = await _context.Sector
                .FirstOrDefaultAsync(m => m.SectorId == id);
            if (sector == null)
            {
                return NotFound();
            }

            return PartialView(sector);
        }

        // POST: Sector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var sector = await _context.Sector.FindAsync(id);
            _context.Sector.Remove(sector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SectorExists(long id)
        {
            return _context.Sector.Any(e => e.SectorId == id);
        }
    }
}
