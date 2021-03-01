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
    public class CitiesController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public CitiesController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Cities
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.City.Include(c => c.Province);
            return View(await learnerContext.ToListAsync());
        }

        public async Task<JsonResult> GetAllCities()
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
 
                
               var listOfCities = await _lookUpService.GetCitiesDetails();

                // Getting all Customer data  z 
                var allProvinces = listOfCities;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allProvinces = allProvinces.Where(m =>
                            m.CityCode == searchValue ||
                            m.CityName == searchValue)
                        as List<CityDetailsDTO>; 
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
        
        // GET: Cities/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var provincesDetails = await  _lookUpService.GetAllCitiesById(id);
            
            if (provincesDetails == null)
            {
                return NotFound();
            }

            return PartialView(provincesDetails);
        }

        // GET: Cities/Create
        public IActionResult Create()
        {
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName");
            return PartialView();
        }

        // POST: Cities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CityId,CityName,CityCode,ProvinceId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] City city)
        {
            if (ModelState.IsValid)
            {
                _context.Add(city);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", city.ProvinceId);
            return PartialView(city);
        }

        // GET: Cities/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var city = await _lookUpService.GetCitiesDetailsByIdForEditDelete(id);
            if (city == null)
            {
                return NotFound();
            }
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", city.ProvinceId);
            return PartialView(city);
        }

        // POST: Cities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CityId,CityName,CityCode,ProvinceId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] City city)
        {
            if (id != city.CityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(city);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CityExists(city.CityId))
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
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", city.ProvinceId);
            return PartialView(city);
        }

        // GET: Cities/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var city = await _lookUpService.GetCitiesDetailsByIdForEditDelete(id);
            
            if (city == null)
            {
                return NotFound();
            }

            return PartialView(city);
        }

        // POST: Cities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var city = await _context.City.FindAsync(id);
            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CityExists(long id)
        {
            return _context.City.Any(e => e.CityId.Equals(id));
        }
    }
}
