using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;

namespace learner_portal.Controllers
{ 
    [Authorize]
    public class AddressController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public AddressController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        public async Task<JsonResult> GetCountryProvinces(long id)
        {
            var  data = await _lookUpService.GetProvincesByCountryId(id);
            
            return Json(data);
        }
        // Get cities by province
        public async Task<JsonResult> GetProvinceCities(long id)
        {
            var  data = await _lookUpService.GetCitiesByProvincesId(id);
            
            return Json(data);
        }
        // Get suburbs by city
        public async Task<JsonResult> GetCitySuburbs(long id)
        {
            var  data = await _lookUpService.GetSuburbsByCityId(id);
            
            return Json(data);
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Address.Include(a => a.AddressType).Include(s => s.Suburb).Include(a => a.City).Include(a => a.Country).Include(a => a.Person).Include(a => a.Province);
            return View(await learnerContext.ToListAsync());
        }
        
           public async Task<JsonResult> GetAllAddress()
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

                var listOfAddress = new List<Address>();

                listOfAddress = await _lookUpService.GetAllAddress();

                // Getting all Customer data  z
                var allInstitution = listOfAddress;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allInstitution = allInstitution.Where(m =>
                            m.City.CityName == searchValue ||
                            m.Province.ProvinceName == searchValue)
                        as List<Address>;
                }

                //total number of rows count   
                recordsTotal = allInstitution.Count();
                //Paging   
                var dataList = allInstitution.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allInstitution });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.AddressType)
                .Include(a => a.City)
                .Include(a => a.Country)
                .Include(a => a.Person)
                .Include(a => a.Province)
                .Include(s => s.Suburb)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Address/Create
        public IActionResult Create()
        {
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeId");
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId");
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbId");
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceId");
            return View();
        }

        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressId,HouseNo,StreetName,SurburbId,CityId,PostalCode,ProvinceId,CountryId,AddressTypeId,PersonId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Address address)
        {
            if (ModelState.IsValid)
            {
                address.CreatedBy = "admin";
                address.DateCreated = DateTime.Now;

                address.LastUpdatedBy = "admin";
                address.DateUpdated = DateTime.Now;
                
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeId", address.AddressTypeId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId", address.CityId);
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbId", address.Suburb);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", address.CountryId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", address.PersonId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceId", address.ProvinceId);
            return View(address);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeId");
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId");
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbId");
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId");
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceId");
            return View(address);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AddressId,HouseNo,StreetName,SurburbId,CityId,PostalCode,ProvinceId,CountryId,AddressTypeId,PersonId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Address address)
        {
            if (id != address.AddressId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                address.CreatedBy = "admin";
                address.DateCreated = DateTime.Now;

                address.LastUpdatedBy = "admin";
                address.DateUpdated = DateTime.Now;
                
                try
                {
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.AddressId))
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
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeId", address.AddressTypeId);
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityId", address.CityId);
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbId", address.Suburb);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryId", address.CountryId);
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId", address.PersonId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceId", address.ProvinceId);
            return View(address);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .Include(a => a.AddressType)
                .Include(a => a.City)
                .Include(a => a.Country)
                .Include(a => a.Person)
                .Include(a => a.Province)
                .FirstOrDefaultAsync(m => m.AddressId == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var address = await _context.Address.FindAsync(id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(long id)
        {
            return _context.Address.Any(e => e.AddressId == id);
        }
    }
}
