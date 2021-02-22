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
    public class OfoMinorController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;

        public OfoMinorController(LearnerContext context, ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: OfoMinor
        public async Task<IActionResult> Index()
        {
            return View(await _context.OfoMinor.ToListAsync());
        }


        public async Task<JsonResult> GetAllOfoMinor()
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

                var listOfOfoMinor = new List<OfoMinorDTO>();
                
                listOfOfoMinor =  await _lookUpService.GetOfoMinorDetails();

                // Getting all Customer data  z
                var allofoMinor = listOfOfoMinor;

                //Search
                if (!string.IsNullOrEmpty(searchValue)) 
                {
                    allofoMinor = allofoMinor.Where(m =>
                            m.OfoMinorCode == searchValue ||
                            m.OfoMinorTitle == searchValue)
                        as List<OfoMinorDTO>;
                }

                //total number of rows count   
                recordsTotal = allofoMinor.Count();
                //Paging   
                var dataList = allofoMinor.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allofoMinor });
            }
            catch (Exception)
            {
                throw;
            }
        }


        // GET: OfoMinor/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var OfoMinorDetails = await _lookUpService.GetOfoMinorById(id);

            if (OfoMinorDetails == null)
            {
                return NotFound();
            }

            return PartialView(OfoMinorDetails);
        }

        // GET: OfoMinor/Create
        public IActionResult Create()
        {
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            return PartialView();
        }

        // POST: OfoMinor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfoMinorId,OfoMinorCode,OfoMinorTitle,OfoSubMajorId,FinancialYearId")] OfoMinor ofoMinor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofoMinor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofoMinor.FinancialYearId);
            return PartialView(ofoMinor);
        }

        // GET: OfoMinor/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var ofoMinor = await _lookUpService.GetOfoMinorByIdForEditDelete(id);

            if (ofoMinor == null) 
            {
                return NotFound();
            }
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            return PartialView(ofoMinor);
        }

        // POST: OfoMinor/Edit/5 
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OfoMinorId,OfoMinorCode,OfoMinorTitle,OfoSubMajorId,FinancialYearId")] OfoMinor ofoMinor)
        {
            if (id != ofoMinor.OfoMinorId)
                {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ofoMinor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfoMinorExists(ofoMinor.OfoMinorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofoMinor.FinancialYearId);
                return RedirectToAction(nameof(Index));
            }
            return PartialView(ofoMinor);
        }

        // GET: OfoMinor/Delete/5
        public async Task<IActionResult> Delete(long id) 
        {
            var ofoMinor = await _lookUpService.GetOfoMinorByIdForEditDelete(id);

            if (ofoMinor == null)
            {
                return NotFound();
            }

            return PartialView(ofoMinor);
        } 

        // POST: OfoMinor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ofoMinor = await _context.OfoMinor.FindAsync(id);
            _context.OfoMinor.Remove(ofoMinor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfoMinorExists(long? id)
        {
            return _context.OfoMinor.Any(e => e.OfoMinorId.Equals(id));
        }

    }
}
 