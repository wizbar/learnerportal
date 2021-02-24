using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using learner_portal.DTO;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers     
{ 
    [Authorize]
    public class OfoUnitController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;

        public OfoUnitController(LearnerContext context, ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: OfoUnit
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfoUnit.ToListAsync());
        }
         

        public async Task<JsonResult> GetAllOfoUnit()
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
 
                var listOfOfoUnit = new List<OfoUnitDTO>(); 

                listOfOfoUnit = await _lookUpService.GetOfoUnitDetails();
 
                // Getting all Customer data  z
                var allofoUnit = listOfOfoUnit;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allofoUnit = allofoUnit.Where(m =>
                            m.OfoUnitCode == searchValue ||
                            m.OfoUnitTitle == searchValue)
                        as List<OfoUnitDTO>;
                }

                //total number of rows count   
                recordsTotal = allofoUnit.Count();
                //Paging   
                var dataList = allofoUnit.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allofoUnit });
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: OfoUnit/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var ofoUnitDetails = await _lookUpService.GetOFOUnitDetailsById(id);
            if (ofoUnitDetails == null)
            {
                return NotFound();
            }

            return PartialView(ofoUnitDetails);
        }

        // GET: OfoUnit/Create
        public IActionResult Create()
        {
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            ViewData["OfoMinorId"] = new SelectList(_context.OfoMinor, "OfoMinorId", "OfoMinorTitle");
            return PartialView();
        }

        // POST: OfoUnit/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfoUnitId,OfoUnitCode,OfoUnitTitle,OfoMinorId,FinancialYearId")] OfoUnit ofoUnit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofoUnit);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofoUnit.FinancialYearId);
            ViewData["OfoMinorId"] = new SelectList(_context.OfoMinor, "OfoMinorId", "OfoMinorTitle", ofoUnit.FinancialYearId);

            return PartialView(ofoUnit);
        }

        // GET: OfoUnit/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var ofoUnit = await _lookUpService.GetOFOUnitDetailsByIdForEditDelete(id);
            if (ofoUnit == null) 
            {
                return NotFound();
            }

            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            ViewData["OfoMinorId"] = new SelectList(_context.OfoMinor, "OfoMinorId", "OfoMinorTitle");

            return PartialView(ofoUnit);
        }

        // POST: OfoUnit/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OfoUnitId,OfoUnitCode,OfoUnitTitle,OfoMinorId,FinancialYearId")] OfoUnit ofoUnit)
        {
            if (id != ofoUnit.OfoUnitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofoUnit);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfoUnitExists(ofoUnit.OfoUnitId))
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
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofoUnit.FinancialYearId);
            ViewData["OfoMinorId"] = new SelectList(_context.OfoMinor, "OfoMinorId", "OfoMinorTitle", ofoUnit.FinancialYearId);
            return PartialView(ofoUnit);
        }


        // GET: OfoUnit/Delete/5
        public async Task<IActionResult> Delete(long id) 
        {
            var ofoUnit = await _lookUpService.GetOFOUnitDetailsByIdForEditDelete(id);
            if (ofoUnit == null)
            {
                return NotFound();
            }

            return PartialView(ofoUnit); 
        }

        // POST: OfoUnit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ofoUnit = await _context.OfoUnit.FindAsync(id);
            _context.OfoUnit.Remove(ofoUnit);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfoUnitExists(long id)
        {
            return _context.OfoUnit.Any(e => e.OfoUnitId.Equals(id));
        }

    }
}
