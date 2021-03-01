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
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{
    [Authorize]
    public class ProvincesController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public ProvincesController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Provinces
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Province.Include(p => p.Country);
            return View(await learnerContext.ToListAsync());
        }
        
        public async Task<JsonResult> GetAllProvinces()
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
 
                
               var listOfProvinces = await _lookUpService.GetProvincesDetails();

                // Getting all Customer data  z 
                var allProvinces = listOfProvinces;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allProvinces = allProvinces.Where(m =>
                            m.ProvinceCode == searchValue ||
                            m.ProvinceName == searchValue)
                        as List<ProvinceDetailsDTO>; 
                }

                //total number of rows count   
                recordsTotal = allProvinces.Count();
                //Paging   
                var data = allProvinces.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allProvinces });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Provinces/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var ProvincesDetails = await  _lookUpService.GetAllProvincesById(id);
            
            if (ProvincesDetails == null)
            {
                return NotFound();
            }

            return PartialView(ProvincesDetails);
        }

        // GET: Provinces/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName");
            return PartialView();
        }

        // POST: Provinces/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProvinceId,ProvinceName,ProvinceCode,CountryId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Province province)
        {
            if (ModelState.IsValid)
            {
                _context.Add(province);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", province.CountryId);
            return PartialView(province);
        }

        // GET: Provinces/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var province = await _lookUpService.GetProvinceDetailsByIdForEditDelete(id);
            
            if (province == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", province.CountryId);
            return PartialView(province);
        }

        // POST: Provinces/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("ProvinceId,ProvinceName,ProvinceCode,CountryId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Province province)
        {
            if (id != province.ProvinceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(province);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProvinceExists(province.ProvinceId))
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
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", province.CountryId);
            return PartialView(province);
        }

        // GET: Provinces/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var province = await _lookUpService.GetProvinceDetailsByIdForEditDelete(id);
            
            if (province == null)
            {
                return NotFound();
            }

            return PartialView(province);
        }

        // POST: Provinces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var province = await _context.Province.FindAsync(id);
            _context.Province.Remove(province);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProvinceExists(long id)
        {
            return _context.Province.Any(e => e.ProvinceId.Equals(id));
        }
    }
}
