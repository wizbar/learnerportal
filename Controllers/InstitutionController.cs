

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learner_portal.DTO;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace learner_portal.Controllers
{
    [Authorize]
    public class InstitutionController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public InstitutionController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: Institution
        public async Task<IActionResult> Index() 
        {
            var learnerContext = _context.Institution.Include(i => i.InstitutionType);
            return View(await learnerContext.ToListAsync());
        }
        
        public async Task<JsonResult> GetAllInstitution()
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

                var listOfInstitution = new List<InstitutionDetailsDTO>();

                listOfInstitution = await _lookUpService.GetInstitutionDetails(); 
  
                // Getting all Customer data  z 
                var allInstitution = listOfInstitution;

                //Search
                if (!string.IsNullOrEmpty(searchValue)) 
                {
                    allInstitution = allInstitution.Where(m =>
                            m.InstitutionCode == searchValue ||
                            m.InstitutionName == searchValue)
                        as List<InstitutionDetailsDTO>;
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

        // GET: Institution/Details/5
        public async Task<IActionResult> Details(long id)
        {
            var institutionDetails = await _lookUpService.GetInstitutionDetailsById(id);
            
            if (institutionDetails == null)
            {
                return NotFound();
            }

            return PartialView(institutionDetails);
        }

        // GET: Institution/Create
        public async Task<IActionResult> Create() 
        {
            ViewData["InstitutionTypeId"] = new SelectList(await _lookUpService.GetAllInstitutionType(), "InstitutionTypeId", "InstitutionTypeDesc");
            return PartialView();
        }

        // POST: Institution/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstitutionId,InstitutionName,InstitutionCode,InstitutionTypeId")] Institution institution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(institution);
                await _context.SaveChangesAsync();
                Alert("Institution created successfully...", learner_portal.Helpers.Enum.NotificationType.success );
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutionTypeId"] = new SelectList(_context.InstitutionType, "InstitutionTypeId", "InstitutionTypeDesc", institution.InstitutionTypeId);
            return PartialView(institution);
        }

        // GET: Institution/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var institution = await _lookUpService.GetInstitutionByIdForEditDelete(id);
            
            if (institution == null)
            {
                return NotFound();
            }
            ViewData["InstitutionTypeId"] = new SelectList(_context.InstitutionType, "InstitutionTypeId", "InstitutionTypeDesc");
            return PartialView(institution);
        }

        // POST: Institution/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("InstitutionId,InstitutionName,InstitutionCode,InstitutionTypeId")] Institution institution)
        {
            if (id != institution.InstitutionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(institution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstitutionExists(institution.InstitutionId))
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
            ViewData["InstitutionTypeId"] = new SelectList(_context.InstitutionType, "InstitutionTypeId", "InstitutionTypeDesc", institution.InstitutionTypeId);
            return PartialView(institution);
        }

        // GET: Institution/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var institution = await _lookUpService.GetInstitutionByIdForEditDelete(id);
            
            if (institution == null)
            {
                return NotFound();
            }

            return PartialView(institution);
        }

        // POST: Institution/Delete/5 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var institution = await _context.Institution.FindAsync(id);
            _context.Institution.Remove(institution);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstitutionExists(long id)
        {
            return _context.Institution.Any(e => e.InstitutionId.Equals(id));
        }
    }
}
