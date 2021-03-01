using System;
using System.Collections.Generic;
using System.Linq; 
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Microsoft.EntityFrameworkCore;  
using learner_portal.Models; 
using learner_portal.Services; 
using Microsoft.AspNetCore.Authorization;
 
namespace learner_portal.Controllers 
{ 
    [Authorize]
    public class JobSectorController : BaseController 
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;
    
        public JobSectorController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService; 
        } 

        // GET: JobSector 
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobSector.ToListAsync());
        }
        
        public JsonResult GetAllJobSector()
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

                var listOfJobSector = new List<JobSector>();

                listOfJobSector = _lookUpService.GetAllJobSector().Result;

                // Getting all Customer data  z
                var alljobSector = listOfJobSector;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    alljobSector = alljobSector.Where(m =>
                            m.JobSectorDesc == searchValue ||
                            m.JobSectorCode == searchValue)
                        as List<JobSector>;
                }

                //total number of rows count   
                recordsTotal = alljobSector.Count();
                //Paging   
                var dataList = alljobSector.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = alljobSector });
            }
            catch (Exception)
            {
                throw;
            } 
        }

        // GET: JobSector/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSector = await _context.JobSector
                .FirstOrDefaultAsync(m => m.JobSectorId == id);
            if (jobSector == null)
            {
                return NotFound();
            }

            return View(jobSector);
        }

        // GET: JobSector/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: JobSector/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobSectorId,JobSectorCode,JobSectorDesc,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] JobSector jobSector)
        {
            jobSector.CreatedBy = "admin";
            jobSector.DateCreated = DateTime.Now;

            jobSector.LastUpdatedBy = "admin";
            jobSector.DateUpdated = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(jobSector);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobSector);
        }

        // GET: JobSector/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
 
            var jobSector = await _context.JobSector.FindAsync(id);
            if (jobSector == null)
            {
                return NotFound();
            }
            return View(jobSector);
        }

        // POST: JobSector/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("JobSectorId,JobSectorCode,JobSectorDesc,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] JobSector jobSector)
        {
            if (id != jobSector.JobSectorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobSector);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobSectorExists(jobSector.JobSectorId))
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
            return View(jobSector);
        }

        // GET: JobSector/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobSector = await _context.JobSector
                .FirstOrDefaultAsync(m => m.JobSectorId == id);
            if (jobSector == null)
            {
                return NotFound();
            }

            return View(jobSector);
        }

        // POST: JobSector/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var jobSector = await _context.JobSector.FindAsync(id);
            _context.JobSector.Remove(jobSector);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobSectorExists(long? id)
        {
            return _context.JobSector.Any(e => e.JobSectorId == id);
        }
    }
}
