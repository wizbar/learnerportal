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
using Microsoft.AspNetCore.Hosting; 
 
namespace learner_portal.Controllers  
{ 
    public class CompaniesController : Controller  
    { 
        private readonly LearnerContext _context;  
        private readonly ILookUpService _lookUpService;
        private readonly FoldersConfigation _fconfig;
        private readonly IFileService  _fileService; 
        private readonly IWebHostEnvironment _env;

        public CompaniesController(LearnerContext context,IWebHostEnvironment env,ILookUpService lookUpService,FoldersConfigation fconfig,IFileService fileService)
        {  
            _context = context;
            _lookUpService = lookUpService;
            _fconfig = fconfig;
            _fileService = fileService;
            _env = env;
        }

        // GET: Companies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Company.ToListAsync());
        }

        public async Task<JsonResult> GetAllCompanies()
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
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0; 

                
            var listOfCompanies = await _lookUpService.GetCompanyDetails().ConfigureAwait(false);

            // Getting all Customer data  z 
            var allCompanies = listOfCompanies;

            //Search
            if (!string.IsNullOrEmpty(searchValue))
            {
                allCompanies = allCompanies.Where(m =>
                        m.CompanyName == searchValue ||
                        m.CityName == searchValue || 
                        m.ProvinceName == searchValue)
                    as List<CompanyDetailsDTO>;
            }

            //total number of rows count   
            recordsTotal = allCompanies.Count(); 
            //Paging    
            var data = allCompanies.Skip(skip).Take(pageSize).ToList(); 
            //Returning Json Data   
            return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allCompanies });
        }
        
    
        // GET: Companies/Details/5
        public async Task<IActionResult> Details(long id)
        { 
   
            var companyDetails = await  _lookUpService.GetCompanyDetailsById(id);
            
            if (companyDetails == null)
            {
                return NotFound();
            }

            return PartialView(companyDetails);
        }
        
        // Create Company 
        public IActionResult CreateCompany()
        {  
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName");
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbName"); 
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName");
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeName");
            
            return View();
        }

        //Create Company
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCompany([Bind("Company", "Address", "Photo")] CompanyViewModel companyViewModel)
        {
            if (ModelState.IsValid)
            {
                
                // This Uploads learner profile image
                var photoPath =  _fconfig.Images  + companyViewModel.Company.CompanyRegistrationNo + "/" + Utils.GenerateImageFolderId() + "/";
                
                if (companyViewModel.Photo != null)
                { 
                    _fileService.UploadFile( companyViewModel.Photo,_env.WebRootPath + photoPath);    
                    
                    companyViewModel.Company.PhotoName = companyViewModel.Photo.FileName;
                    companyViewModel.Company.PhotoPath = photoPath;
                    Console.WriteLine(" FILE NAME : " + companyViewModel.Photo.FileName); 
                }

                companyViewModel.Company.CreatedBy = User.Identity.Name;
                companyViewModel.Company.DateCreated = DateTime.Now;
                companyViewModel.Company.LastUpdatedBy = User.Identity.Name;
                companyViewModel.Company.DateUpdated = DateTime.Now;
                 
                companyViewModel.Address.CreatedBy =  User.Identity.Name;
                companyViewModel.Address.DateCreated = DateTime.Now;
                companyViewModel.Address.LastUpdatedBy = User.Identity.Name;
                companyViewModel.Address.DateUpdated = DateTime.Now; 
                
                companyViewModel.Company.Address.Add(companyViewModel.Address);
                
                //Add company to the database     
                _context.Add(companyViewModel.Company);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Index));
            } 
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", companyViewModel.Address.CityId);
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbName", companyViewModel.Address.SuburbId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", companyViewModel.Address.CountryId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", companyViewModel.Address.ProvinceId);
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeName", companyViewModel.Address.AddressTypeId);
             
            return View(companyViewModel);
        }

        // GET: Companies/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: Companies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CompanyId,CompanyName,CompanyRegistrationNo,DateBusinessCommenced,ContactName,ContactSurname,ContactEmail,ContactMobile,ContactTelephone,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(company);
        }

        // GET: Companies/Edit/5 
        public async Task<IActionResult> Edit(long id) 
        {

            var company = await  _lookUpService.GetCompanyDetailsByIdForEditDelete(id);
            
            if (company == null)
            {
                return NotFound();
            }
            ViewData["CityId"] = new SelectList(_context.City, "CityId", "CityName", company.Address.CityId);
            ViewData["SuburbId"] = new SelectList(_context.Suburb, "SuburbId", "SuburbName", company.Address.SuburbId);
            ViewData["CountryId"] = new SelectList(_context.Country, "CountryId", "CountryName", company.Address.CountryId); 
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", company.Address.ProvinceId);
            ViewData["AddressTypeId"] = new SelectList(_context.AddressType, "AddressTypeId", "AddressTypeName", company.Address.AddressTypeId);
            return PartialView(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public async Task<IActionResult> Edit(long id, [Bind("Company", "Address")] CompanyViewModel companyViewModel)
        {

            if (ModelState.IsValid) 
            {
                
                companyViewModel.Company.LastUpdatedBy = "admin";
                companyViewModel.Company.DateUpdated = DateTime.Now;

                var cmp = companyViewModel.Company;
                cmp.Address.Add(companyViewModel.Address);
                
                try
                {
                    _context.Update(cmp);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(companyViewModel.Company.CompanyId))
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
            return PartialView(companyViewModel);
        }
 
        // GET: Companies/Delete/5 
        public async Task<IActionResult> Delete(long id)
        {

            var company =  await _lookUpService.GetCompanyDetailsByIdForEditDelete(id);
            
            if (company.Company.CompanyRegistrationNo == null)
            { 
                return NotFound();
            }

            return PartialView(company.Company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var company = await _context.Company.FindAsync(id);
            _context.Company.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(long id)
        {
            return _context.Company.Any(e => e.CompanyRegistrationNo.Equals(id));
        }
    }
}
