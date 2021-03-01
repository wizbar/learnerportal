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
    public class AddressTypesController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public AddressTypesController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: AddressTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.AddressType.ToListAsync());
        }
        
        public async Task<JsonResult> GetAllAddressTypes()
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
 
                
               var listOfAddressTypes = await _lookUpService.GetAddressTypeDetails();

                // Getting all Customer data  z 
                var allAddressTypes = listOfAddressTypes;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allAddressTypes = allAddressTypes.Where(m =>
                            m.AddressTypeCode == searchValue ||
                            m.AddressTypeName == searchValue)
                        as List<AddressTypeDetailsDTO>; 
                }

                //total number of rows count   
                recordsTotal = allAddressTypes.Count();
                //Paging   
                var data = allAddressTypes.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allAddressTypes });
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        // GET: AddressTypes/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var addressTypesDetails = await  _lookUpService.GetAllAddressTypeById(id);
            
            if (addressTypesDetails == null)
            {
                return NotFound();
            }

            return PartialView(addressTypesDetails);
        }

        // GET: AddressTypes/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: AddressTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AddressTypeId,AddressTypeName,AddressTypeCode,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] AddressType addressType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                    throw new Exception("Please correct the following errors: " + Environment.NewLine + messages);
                }

                _context.Add(addressType);
                await _context.SaveChangesAsync();
           //     return RedirectToAction(nameof(Index));

                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        // GET: AddressTypes/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var addressType = await _lookUpService.GetAddressTypeDetailsByIdForEditDelete(id);
            if (addressType == null)
            {
                return NotFound();
            }
            return PartialView(addressType);
        }

        // POST: AddressTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("AddressTypeId,AddressTypeName,AddressTypeCode,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] AddressType addressType)
        {
            if (id != addressType.AddressTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(addressType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressTypeExists(addressType.AddressTypeId))
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
            return PartialView(addressType);
        }

        // GET: AddressTypes/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var addressType = await _lookUpService.GetAddressTypeDetailsByIdForEditDelete(id);
            
            if (addressType == null)
            {
                return NotFound();
            }

            return PartialView(addressType);
        }

        // POST: AddressTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var addressType = await _context.AddressType.FindAsync(id);
            _context.AddressType.Remove(addressType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressTypeExists(long id)
        {
            return _context.AddressType.Any(e => e.AddressTypeId.Equals(id));
        }
    }
}
