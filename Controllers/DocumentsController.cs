using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
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
    public class DocumentsController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly ILookUpService _lookUpService;
        private readonly IFileService _fileService;
        private readonly UserManager<Users> _userManager; 
        private readonly FoldersConfigation _foldersConfigation;        
        private readonly INotyfService _notyf;
        

        public DocumentsController(LearnerContext context,
            ILookUpService lookUpService,
            IFileService fileService,
            FoldersConfigation foldersConfigation,
            UserManager<Users> userManager,
            INotyfService notyf)
        {
            _context = context;
            _lookUpService = lookUpService;
            _fileService = fileService;
            _userManager = userManager;
            _foldersConfigation = foldersConfigation;
            _notyf = notyf;
        }
        
        public async  Task<IActionResult> DownloadDocument(Guid id)
        {

            // Checks if the id.

            var document = await _context.Document.Where(d => d.Id == id).FirstOrDefaultAsync();
            
            byte[] fileBytes = new byte[] { };
            
            if (ModelState.IsValid)
            {
                var path =   document.FilePath + "/" + document.FileName;
                fileBytes =  _fileService.DownloadFile(path).Data;
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
                
                if (!ModelState.IsValid)
                {
                    string messages = string.Join("; ", ModelState.Values
                        .SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage));
                    throw new Exception("Please correct the following errors: " + Environment.NewLine + messages);
                }
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

                if (listOfDocuments == null)
                {
                    return Json(new { Result = "ERROR", Message ="Oops something went wrong..."});
                }

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
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
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
        
        public async Task<IActionResult> _Verify(Guid id)
        {
            var document = await _lookUpService.GetDocumentsDetailsByIdForEditDelete(id);
            if ( document == null)
            {
                return NotFound();
            }
            var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
            var userRole =  _userManager.GetRolesAsync(user).Result;
            
            ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
            ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName");
            ViewData["LearnerId"] = new SelectList(_context.Learner, "LearnerId", "LearnerId");
            ViewData["Id"] = new SelectList(_context.JobApplications, "Id", "Id");
            return View(document);
        }
        
                [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> _Verify(Guid id, [Bind("Id,FilePath,FileName,DocumentTypeId,Comments,LearnerId")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }
   
            if (ModelState.IsValid)
            {
                document.Verified = Const.TRUE;
                document.VerifiedBy = User.Identity.Name;
                document.VerificationDate = DateTime.Now;
                document.LastUpdatedBy = User.Identity.Name;
                document.DateUpdated = DateTime.Now;
 
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Success("Document verified successfully....");
                var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
                return RedirectToAction("Details","Person", new {id = user.Person.NationalId});
            }

            return View(document);
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
                    document.FilePath =  _foldersConfigation.Documents + learner.NationalID + "/" +
                                        (_lookUpService.GetAllDocumentTypesById(document.DocumentTypeId).Result
                                            .TypeName);
                _fileService.UploadFile(document.MyFiles,document.FilePath);

                //create an audit trail
                document.CreatedBy = user.UserName;
                document.DateCreated = DateTime.Now;
                document.LastUpdatedBy = user.UserName;
                document.DateUpdated = DateTime.Now;
                document.Verified = "false";
                document.FilePath =  _foldersConfigation.Documents + learner.NationalID + "/" +
                                    (_lookUpService.GetAllDocumentTypesById(document.DocumentTypeId).Result
                                        .TypeName) + "/";
   
                _context.Add(document);
                await _context.SaveChangesAsync();     
                _notyf.Success("Document created successfully....");
                
                return RedirectToAction("Details","Person", new {id = learner.NationalID});
            } 
  
            return PartialView(document);
        }
        
        // GET: document/Create
        public async Task<IActionResult> _AddDocuments()
        {
            var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
            var userRole =  _userManager.GetRolesAsync(user).Result;
           
            ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
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
                _notyf.Success("Document added successfully....");
                return RedirectToAction(nameof(Index)); 
            }

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
            var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
            var userRole =  _userManager.GetRolesAsync(user).Result;
            
            ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
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
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,FilePath,FileName,DocumentTypeId,Comments,LearnerId,CompanyId,Verified,VerificationDate,VerifiedBy,JobApplicationId,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated,MyFiles")] Document document)
        {
            if (id != document.Id)
            {
                return NotFound();
            }
   
            if (ModelState.IsValid)
            {
                document.Verified = Const.FALSE;
                document.LastUpdatedBy = User.Identity.Name;
                document.DateUpdated = DateTime.Now;
                string path = document.FilePath + document.FileName;
                if (_fileService.FileExists(path))
                {
                    _fileService.DeleteFile(path);
                }
                  _fileService.UploadFile(document.MyFiles, document.FilePath );
                try
                {
                    document.FileName = document.MyFiles.FileName;
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FileExists(document.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _notyf.Success("Document edited successfully....");
                var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
                return RedirectToAction("Details","Person", new {id = user.Person.NationalId});
            }
            /*ViewData["CompanyId"] = new SelectList(_context.Company, "CompanyId", "CompanyName", document.CompanyId);
            ViewData["LearnerId"] = new SelectList(_context.Learner, "LearnerId", "LearnerId", document.LearnerId);
            ViewData["Id"] = new SelectList(_context.JobApplications, "Id", "Id", document.JobApplicationId);*/
            return View(document);
        }

        // GET: Address/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
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
            
            var person = await _lookUpService.GetLearnerDetailsById(document.LearnerId);
            _notyf.Success("Document deleted successfully....");
            return RedirectToAction("Details","Person", new { Id = person.NationalID});
        }

        private bool FileExists(Guid id)
        {
            return _context.Document.Any(e => e.Id.Equals(id));
        }
    }
}
