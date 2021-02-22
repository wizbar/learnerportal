using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.DTO;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{
    [Authorize]
    public class OfoController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;

        public OfoController(LearnerContext context, ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Ofo
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Ofo.Include(o => o.Financialyear).Include(o => o.OfoUnit);
            return View(await learnerContext.ToListAsync());
        }


        public async Task<JsonResult> GetAllOfo()
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

                var listOfOfo = new List<OfoDTO>();

                listOfOfo = await _lookUpService.GetOFODetails();

                // Getting all Customer data  z
                var allofo = listOfOfo;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allofo = allofo.Where(m =>
                            m.OfoCode == searchValue ||
                            m.OfoTitle == searchValue||
                            m.FinancialyearName == searchValue)
                        as List<OfoDTO>;






                }

                //total number of rows count   
                recordsTotal = allofo.Count();
                //Paging   
                var dataList = allofo.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allofo });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Ofo/Details/5
        public async Task<IActionResult> Details(long id)
        {
            if (id == null)
            {
                return NotFound();
            }
             
            var ofoDetails = await _lookUpService.GetOFODetailsById(id);

            if (ofoDetails == null)
            {
                return NotFound();
            }

            return PartialView(ofoDetails);
        }

        // GET: Ofo/Create
        public IActionResult Create()
        {
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            ViewData["OfoUnitId"] = new SelectList(_context.OfoUnit, "OfoUnitId", "OfoUnitTitle");
            return PartialView();
        }

        // POST: Ofo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OfoId,OfoCode,OfoTitle,OfoUnitId,FinancialYearId")] Ofo ofo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ofo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofo.FinancialYearId);
            ViewData["OfoUnitId"] = new SelectList(_context.OfoUnit, "OfoUnitId", "OfoUnitTitle", ofo.OfoUnitId);
            return PartialView(ofo);
        }

        // GET: Ofo/Edit/5
        public async Task<IActionResult> Edit(long id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var ofo = await _lookUpService.GetOFODetailsByIdForEditDelete(id);
            if (ofo == null)
            {
                return NotFound();
            }
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc");
            ViewData["OfoUnitId"] = new SelectList(_context.OfoUnit, "OfoUnitId", "OfoUnitTitle");
            return PartialView(ofo);
        }

        // POST: Ofo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("OfoId,OfoCode,OfoTitle,OfoUnitId,FinancialYearId")] Ofo ofo)
        {
            if (id != ofo.OfoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
              /*  ofo.LastUpdatedBy = "admin";
                ofo.DateUpdated = DateTime.Now;*/

                try 
                {
                    _context.Update(ofo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OfoExists(ofo.OfoCode))
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
            ViewData["FinancialYearId"] = new SelectList(_context.Financialyear, "FinancialyearId", "FinancialyearDesc", ofo.FinancialYearId);
            ViewData["OfoUnitId"] = new SelectList(_context.OfoUnit, "OfoUnitId", "OfoUnitITitle", ofo.OfoUnitId);
            return PartialView(ofo);
        }

        // GET: Ofo/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            if (id == null)
            {
                return NotFound(); 
            }

            var ofo = await _lookUpService.GetOFODetailsByIdForEditDelete(id);
            if (ofo == null) 
            {
                return NotFound();
            }

            return PartialView(ofo); 
        }

        // POST: Ofo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var ofo = await _context.Ofo.FindAsync(id);
            _context.Ofo.Remove(ofo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OfoExists(string id)
        {
            return _context.Ofo.Any(e => e.OfoId.Equals(id));
        }
    }
}
