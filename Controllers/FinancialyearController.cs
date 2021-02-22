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
    public class FinancialyearController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;

        public FinancialyearController(LearnerContext context, ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }
         
        // GET: Financialyear
        public async Task<IActionResult> Index()
        {
            return View(await _context.Financialyear.ToListAsync());
        }


        public JsonResult GetAllFinancialyear()
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

                var listOfFinancialyear = new List<Financialyear>();

                listOfFinancialyear = _lookUpService.GetAllFinancialyear().Result;

                // Getting all Customer data  z
                var allfinancialyear = listOfFinancialyear;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allfinancialyear = allfinancialyear.Where(m =>
                            m.FinancialyearDesc == searchValue ||
                            m.ActiveForWsp == searchValue)
                        as List<Financialyear>;
                }

                //total number of rows count   
                recordsTotal = allfinancialyear.Count();
                //Paging   
                var dataList = allfinancialyear.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allfinancialyear });
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: Financialyear/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialyear = await _context.Financialyear
                .FirstOrDefaultAsync(m => m.FinancialyearId == id);
            if (financialyear == null)
            {
                return NotFound();
            }

            return PartialView(financialyear);
        }

        // GET: Financialyear/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Financialyear/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FinancialyearId,StartDate,EndDate,ActiveForWsp,FinancialyearDesc,LockedForWspSubmission,ActiveYn")] Financialyear financialyear)
        {
            if (ModelState.IsValid)
            {
                _context.Add(financialyear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(financialyear);
        }

        // GET: Financialyear/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialyear = await _context.Financialyear.FindAsync(id);
            if (financialyear == null)
            {
                return NotFound();
            }
            return PartialView(financialyear);
        }

        // POST: Financialyear/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("FinancialyearId,StartDate,EndDate,ActiveForWsp,FinancialyearDesc,LockedForWspSubmission,ActiveYn")] Financialyear financialyear)
        {
            if (id != financialyear.FinancialyearId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(financialyear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FinancialyearExists(financialyear.FinancialyearId))
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
            return PartialView(financialyear);
        }

        // GET: Financialyear/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var financialyear = await _context.Financialyear
                .FirstOrDefaultAsync(m => m.FinancialyearId == id);
            if (financialyear == null)
            {
                return NotFound();
            }

            return PartialView(financialyear);
        }

        // POST: Financialyear/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var financialyear = await _context.Financialyear.FindAsync(id);
            _context.Financialyear.Remove(financialyear);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FinancialyearExists(long id)
        {
            return _context.Financialyear.Any(e => e.FinancialyearId == id);
        }
    }
}
