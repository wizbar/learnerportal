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
    public class CountriesController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public CountriesController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }
 
        // GET: Countries 
        public async Task<IActionResult> Index()
        { 
            return View(await _context.Country.ToListAsync());
        }
        
        public async Task<JsonResult> GetAllCountries()
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
 
                
               var listOfCountries = await _lookUpService.GetCountriesDetails();

                // Getting all Customer data  z 
                var allCountries = listOfCountries;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allCountries = allCountries.Where(m =>
                            m.CountryCode == searchValue ||
                            m.CountryName == searchValue)
                        as List<CountriesDetailsDTO>; 
                }

                //total number of rows count   
                recordsTotal = allCountries.Count();
                //Paging   
                var data = allCountries.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allCountries });
            }
            catch (Exception)
            {
                throw;
            }  
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var CountriesDetails = await  _lookUpService.GetCountriesDetailsById(id);
            
            if (CountriesDetails == null)
            {
                return NotFound();
            }

            return PartialView(CountriesDetails);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CountryId,CountryName,CountryCode,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Country country)
        {
            if (ModelState.IsValid)
            {
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var country = await _lookUpService.GetCountriesDetailsByIdForEditDelete(id);
            
            if (country == null)
            {
                return NotFound();
            }
            return PartialView(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CountryId,CountryName,CountryCode,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Country country)
        {
            if (id != country.CountryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.CountryId))
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
            return PartialView(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var country = await _lookUpService.GetCountriesDetailsByIdForEditDelete(id);
            
            if (country == null)
            {
                return NotFound();
            }

            return PartialView(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var country = await _context.Country.FindAsync(id);
            _context.Country.Remove(country);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(long id)
        {
            return _context.Country.Any(e => e.CountryId.Equals(id));
        }
    }
}
