using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using learner_portal.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace learner_portal.Controllers
{
    [Authorize]
    public class JobController : Controller
    {
        private readonly LearnerContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly  ILookUpService _lookUpService;

        public JobController(LearnerContext context, IWebHostEnvironment env,ILookUpService lookUpService)
        { 
            _context = context;
            _env = env;
            _lookUpService = lookUpService;
        }

        // GET: Job
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Jobs.Include(t =>t.JobType).Include(s => s.Sector).ToListAsync());
        }
        
        public async Task<IActionResult> IndexAdmin()
        {
            return View(await _context.Jobs.Include(t =>t.JobType).Include(s => s.Sector).ToListAsync());
        }

        
        
        private bool SingleFile(string path,IFormFile file)
        {

               // Console.WriteLine("WRITING FILE : " + file.FileName);
                using (var filestream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create, FileAccess.Write))
                {
                    file.CopyTo(filestream);
                }

            return true;

        }

        public async Task<JsonResult> GetAllJob()
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
                

                var listOfJob = await _lookUpService.GetJobDetails();

                // Getting all Customer data  z
                var allJob = listOfJob;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    allJob = allJob.Where(m =>
                            m.JobCode == searchValue ||
                            m.JobDesc == searchValue)
                        as List<JobDetailsDTO>;
                }

                //total number of rows count   
                recordsTotal = allJob.Count();
                //Paging   
                var dataList = allJob.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = allJob });
            }
            catch (Exception)
            {
                throw;
            } 
        } 
        
        // GET: Job/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(long id)
        {
            var jobDetails = await  _lookUpService.GetJobDetailsById(id);
            
            if (jobDetails == null)
            {
                return NotFound();
            }

            return PartialView(jobDetails);
        }
        
        // GET: Job/Details/5
        /*public async Task<IActionResult> _JobDetails(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobDetails = await  _lookUpService.GetJobDetailsById(id);
            
            if (jobDetails == null)
            {
                return NotFound();
            }

            return View(jobDetails);
        }*/
        
        // GET: Job/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> _JobDetails(long id)
        {
            var jobDetails = await  _lookUpService.GetJobDetailsById(id);
            
            if (jobDetails == null)
            {
                return NotFound();
            }

            return PartialView(jobDetails);
        }

        // GET: Job/Create
        public async Task<IActionResult> Create()
        {
            Debug.Assert(_lookUpService != null, nameof(_lookUpService) + " != null");
            ViewData["CompanyId"] = new SelectList( await _lookUpService?.GetCompanyDetails(), "CompanyId", "CompanyName");
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "JobTypeId", "JobTypeDesc");
            ViewData["JobSectorId"] = new SelectList(_context.JobSector, "JobSectorId", "JobSectorDesc");
            ViewData["SectorId"] = new SelectList(_context.Sector, "SectorId", "SectorDesc");
            ViewData["OfoId"] = new SelectList(_context.Ofo, "OfoId", "OfoTitle");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName");
            return PartialView();
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,JobCode,JobTitle,JobDesc,JobTypeId,OfoId,JobSectorId,SectorId,ProvinceId,ListedDate,ExpiryDate,JobPhoto,JobPhotoPath,JobPhotoName")] Job job)
        {
            if (ModelState.IsValid)
            {
                var jobPhotoPath =  "/Images/" + System.Guid.NewGuid().ToString().Substring(0,10) + "/";
                 
                if (!Directory.Exists(_env.WebRootPath + jobPhotoPath))
                {
                    Directory.CreateDirectory(_env.WebRootPath + jobPhotoPath);
                }

                SingleFile(_env.WebRootPath + jobPhotoPath,job.JobPhoto);

                job.JobPhotoName = job.JobPhoto.FileName;
                job.JobPhotoPath = jobPhotoPath;
                
                Console.WriteLine(" FILE NAME : " + job.JobPhoto.FileName);
                
                _context.Add(job);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "JobTypeId", "JobTypeDesc", job.JobTypeId);
            ViewData["JobSectorId"] = new SelectList(_context.JobSector, "JobSectorId", "JobSectorDesc", job.JobSectorId);
            ViewData["SectorId"] = new SelectList(_context.Sector, "SectorId", "SectorDesc", job.SectorId);
            ViewData["OfoId"] = new SelectList(_context.Ofo, "OfoId", "OfoTitle", job.JobSectorId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", job.ProvinceId);
            return PartialView(job);
        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            var job = await _lookUpService.GetJobDetailsByIdForEditDelete(id);
            
            if (job == null)
            {
                return NotFound();
            }
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "JobTypeId", "JobTypeDesc");
            ViewData["JobSectorId"] = new SelectList(_context.JobSector, "JobSectorId", "JobSectorDesc");
            ViewData["SectorId"] = new SelectList(_context.Sector, "SectorId", "SectorDesc");
            ViewData["OfoId"] = new SelectList(_context.Ofo, "OfoId", "OfoTitle");
            ViewData["CompanyId"] = new SelectList( await _lookUpService?.GetCompanyDetails(), "CompanyId", "CompanyName");
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName");

            return PartialView(job);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("JobId,JobCode,JobTitle,JobDesc,JobTypeId,OfoId,JobSectorId,SectorId,ProvinceId,ListedDate,ExpiryDate")] Job job)
        {
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.JobId))
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
            ViewData["JobTypeId"] = new SelectList(_context.JobType, "JobTypeId", "JobTypeDesc", job.JobTypeId);
            ViewData["JobSectorId"] = new SelectList(_context.JobSector, "JobSectorId", "JobSectorDesc", job.JobSectorId);
            ViewData["SectorId"] = new SelectList(_context.Sector, "SectorId", "SectorDesc", job.JobSectorId);
            ViewData["OfoId"] = new SelectList(_context.Ofo, "OfoId", "OfoTitle", job.OfoId);
            ViewData["CompanyId"] = new SelectList( await _lookUpService?.GetCompanyDetails(), "CompanyId", "CompanyName", job.CompanyId);
            ViewData["ProvinceId"] = new SelectList(_context.Province, "ProvinceId", "ProvinceName", job.ProvinceId);
            return PartialView(job); 
        }

        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            var job = await _lookUpService.GetJobDetailsByIdForEditDelete(id);

            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var job = await _context.Jobs.FindAsync(id);
            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(long id)
        {
            return _context.Jobs.Any(e => e.JobId.Equals(id));
        }
    }
}
