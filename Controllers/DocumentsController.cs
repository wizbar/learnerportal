using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using learner_portal.DTO;
using learner_portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace learner_portal.Controllers
{ 
    [Authorize]
    public class DocumentsController : Controller
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;
        private readonly IFileService _fileService;
        private readonly IWebHostEnvironment _env;
        private readonly FoldersConfigation _foldersConfigation;

        public DocumentsController(LearnerContext context,ILookUpService lookUpService,IFileService fileService,IWebHostEnvironment env,FoldersConfigation foldersConfigation)
        {
            _context = context;
            _lookUpService = lookUpService;
            _fileService = fileService;
            _env = env;
            _foldersConfigation = foldersConfigation;
        }
        
        public async  Task<IActionResult> DownloadDocument(Guid id)
        {

            // Checks if the id.

            var document = await _context.Document.Where(d => d.Id == id).FirstOrDefaultAsync();
            
            byte[] fileBytes = new byte[] { };
            
            if (ModelState.IsValid)
            {
                var path =  _env.WebRootPath + document.FilePath + "/" + document.FileName;
                fileBytes =  _fileService.DownloadDocument(path).Data;
            }
          
            return File(fileBytes, "application/pdf"); 
        }

        // GET: Address
        public async Task<IActionResult> Index()
        {
            var learnerContext = _context.Address;
            return View(await learnerContext.ToListAsync());
        }
        
           public async Task<JsonResult> GetAllDocuments()
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

                var listOfDocuments = new List<DocumentDetailsDTO>();

                listOfDocuments = await _lookUpService.GetDocumentsDetails();
  
                // Getting all Customer data  z 
                var allDocuments = listOfDocuments;

                //Search
                if (!string.IsNullOrEmpty(searchValue)) 
                {
                    allDocuments = allDocuments.Where(m =>
                            m.CompanyName == searchValue ||
                            m.CompanyName == searchValue)
                        as List<DocumentDetailsDTO>;
                }

                //total number of rows count   
                recordsTotal = allDocuments.Count();
                //Paging   
                var dataList = allDocuments.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allDocuments });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: Address/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var document = await _lookUpService.GetDocumentById(id);
            
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Address/Create
        public IActionResult Create()
        {

            return PartialView();
        }


        // POST: Address/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FilePath,FileName,DocumentTypeId,Comments,MyFiles,LearnerId,CompanyId,Verified,VerificationDate,VerifiedBy,JobApplicationId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Document document)
        {
            
            
            if (ModelState.IsValid)
            {
                //Get current user details 
                var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
                //Get current leaner details
                var learner = await _lookUpService.GetLearnerDetailsByIdEmail(user.Person.NationalId);
             
                //assign Leaner Id to link these qualifications to leaner 
                document.LearnerId = learner.LearnerId;

                document.FileName = document.MyFiles.FileName;
                if (document.DocumentTypeId != null)
                    document.FilePath = _env.WebRootPath + _foldersConfigation.Docs + learner.NationalID + "/" +
                                        (_lookUpService.GetAllDocumentTypesById(document.DocumentTypeId).Result
                                            .TypeName);
                _fileService.UploadFile(document.MyFiles,document.FilePath);
                
             
                //create an audit trail
                document.CreatedBy = user.UserName;
                document.DateCreated = DateTime.Now;
                document.LastUpdatedBy = user.UserName;
                document.DateUpdated = DateTime.Now;
                document.Verified = "false";
                document.FilePath =  _foldersConfigation.Docs + learner.NationalID + "/" +
                                    (_lookUpService.GetAllDocumentTypesById(document.DocumentTypeId).Result
                                        .TypeName) + "/";
   
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details","Person", new {id = learner.NationalID});
            } 
  
            return PartialView(document);
        }
        
        // GET: document/Create
        public async Task<IActionResult> _AddDocuments()
        {
            
                return PartialView();
            

        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _AddDocuments([Bind("Id","FilePath","FileName","DocumentTypeId","Comments","Verified","VerificationDate","JobApplicationId")] Document document)
        {
            //Get current user details 
            var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
            //Get current leaner details
            var learner = await _lookUpService.GetLearnerDetailsByIdEmail(user.Person.NationalId);
             
            //assign Leaner Id to link these qualifications to leaner 
            document.LearnerId = learner.LearnerId;
             
            //create an audit trailss
            document.VerifiedBy = user.UserName;
            document.CreatedBy = user.UserName;
            document.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {  
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }

            
            ViewData["DocumentTypeId"] = new SelectList(await _lookUpService.GetCities(), "id", "Name");
            return PartialView(document);
        }

        // GET: Address/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var document = await _lookUpService.GetDocumentsDetailsByIdForEditDelete(id);
            if ( document == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.DocumentType, "Id", "TypeName");
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName");
            ViewData["LearnerId"] = new SelectList(_context.Learner, "LearnerId", "LearnerId");
            ViewData["Id"] = new SelectList(_context.JobApplications, "Id", "Id");
            return View(document);
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FilePath,FileName,DocumentTypeId,Comments,LearnerId,CompanyId,Verified,VerificationDate,VerifiedBy,JobApplicationId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                document.LastUpdatedBy = User.Identity.Name;
                document.DateUpdated = DateTime.Now;
                
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(document.Id))
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
            ViewData["Id"] = new SelectList(_context.DocumentType, "Id", "TypeName", document.DocumentTypeId);
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName", document.CompanyId);
            ViewData["LearnerId"] = new SelectList(_context.Learner, "LearnerId", "LearnerId", document.LearnerId);
            ViewData["Id"] = new SelectList(_context.JobApplications, "Id", "Id", document.JobApplicationId);
            return View(document);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _lookUpService.GetDocumentsDetailsByIdForEditDelete(id);
            
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var document = await _context.Document.FindAsync(id);
            _context.Document.Remove(document);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(Guid id)
        {
            return _context.Document.Any(e => e.Id.Equals(id));
        }
    }
}
