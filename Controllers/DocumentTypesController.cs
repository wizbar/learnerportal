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
using Microsoft.AspNetCore.Identity;

namespace learner_portal.Controllers
{
    public class DocumentTypesController : Controller
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;
    
        public DocumentTypesController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: DocumentTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.DocumentType.ToListAsync());
        }
        
        public async Task<JsonResult> GetAllDocumentTypes()
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
 
                
               var listOfDocumentTypes = await _lookUpService.GetDocumentTypesDetails();

                // Getting all Customer data  z 
                var allDocumentTypes = listOfDocumentTypes;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allDocumentTypes = allDocumentTypes.Where(m =>
                            m.TypeName == searchValue ||
                            m.Description == searchValue)
                        as List<DocumentTypesDetailsDTO>; 
                }

                //total number of rows count   
                recordsTotal = allDocumentTypes.Count();
                //Paging   
                var data = allDocumentTypes.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allDocumentTypes });
            }
            catch (Exception)
            {
                throw;
            }
        }
        

        // GET: DocumentTypes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var documentTypesDetails = await  _lookUpService.GetAllDocumentTypesById(id);
            
            if (documentTypesDetails == null)
            {
                return NotFound();
            }

            return PartialView(documentTypesDetails);
        } 

        // GET: DocumentTypes/Create
        public IActionResult Create()
        {
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            return PartialView();
        }

        // POST: DocumentTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeName,ActiveYn,RoleId,Description,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] DocumentType documentType)
        {
            if (ModelState.IsValid)
            {
                
                documentType.CreatedBy = User.Identity.Name;
                documentType.DateCreated = DateTime.Now;
                
                _context.Add(documentType);  
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
          //  ViewData["RoleId"] = new SelectList(_context.Role, "RoleId", "Name",documentType.RoleId);
            return PartialView(documentType);
        } 
    
        // GET: DocumentTypes/Edit/5 
        public async Task<IActionResult> Edit(Guid id)
        {
            var documentType = await _lookUpService.GetDocumentTypesDetailsByIdForEditDelete(id);
            if (documentType == null)
            {
                return NotFound();
            }
            
            ViewData["RoleId"] = new SelectList(_context.Role, "Id", "Name");
            
            return PartialView(documentType);
        }

        // POST: DocumentTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TypeName,ActiveYn,RoleId,Description,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] DocumentType documentType)
        {
            if (id != documentType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                documentType.LastUpdatedBy = User.Identity.Name;
                documentType.DateUpdated = DateTime.Now;
                
                try
                {
                    _context.Update(documentType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressTypeExists(documentType.Id))
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
            return PartialView(documentType);
        }

        // GET: DocumentTypes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var documentType = await _lookUpService.GetDocumentTypesDetailsByIdForEditDelete(id);
            
            if (documentType == null)
            {
                return NotFound();
            }

            return PartialView(documentType);
        }

        // POST: DocumentTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var documentType = await _context.DocumentType.FindAsync(id);
            _context.DocumentType.Remove(documentType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressTypeExists(Guid id)
        {
            return _context.DocumentType.Any(e => e.Id.Equals(id));
        }
    }
}
