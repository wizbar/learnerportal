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
    public class JobTypeController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;

        public JobTypeController(LearnerContext context,ILookUpService lookUpService)
        {
            _context = context;
            _lookUpService = lookUpService;
        }

        // GET: JobType
        public async Task<IActionResult> Index()
        {
            return View(await _context.JobType.ToListAsync());
        }

                public JsonResult GetAllJobType() 
        {
            try
            {
                var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
                // Skiping number of Rows count  
                var start = Request.Query["start"].FirstOrDefault();
                // Paging Length 10,20  
                var length = Request.Query["length"].FirstOrDefault();
                // Sort Column Name  
                // Sort Column Direction ( asc ,desc)  
                var sortColumnDirection = Request.Query["order[0][dir]"].FirstOrDefault();
                // Search Value from (Search box)  
                var searchValue = Request.Query["search[value]"].FirstOrDefault();

                //Paging Size (10,20,50,100)  
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var recordsTotal = 0;

                var listOfJobType = new List<JobType>();

                listOfJobType = _lookUpService.GetAllJobType().Result;

                // Getting all Customer data  z
                var alljobType = listOfJobType;

                //Search
                if (!string.IsNullOrEmpty(searchValue))
                {
                    alljobType = alljobType.Where(m =>
                            m.JobTypeCode == searchValue ||
                            m.JobTypeDesc == searchValue)
                        as List<JobType>;
                }

                //total number of rows count   
                recordsTotal = alljobType.Count();
                //Paging   
                var dataList = alljobType.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new 
                { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = alljobType });
            }
            catch (Exception)
            {
                throw;
            } 
        }
        
        // GET: JobType/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobType
                .FirstOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound();
            }

            return PartialView(jobType);
        }

        // GET: JobType/Create
        public IActionResult Create()
        {
            return PartialView();
        }

        // POST: JobType/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobTypeId,JobTypeCode,JobTypeDesc,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] JobType jobType)
        {
            jobType.CreatedBy = "admin";
            jobType.DateCreated = DateTime.Now;

            jobType.LastUpdatedBy = "admin";
            jobType.DateUpdated = DateTime.Now;
            
            if (ModelState.IsValid)
            {
                _context.Add(jobType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return PartialView(jobType);
        }

        // GET: JobType/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobType = await _context.JobType.FindAsync(id);
            if (jobType == null)
            {
                return NotFound();
            }
            return PartialView(jobType);
        }

        // POST: JobType/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("JobTypeId,JobTypeCode,JobTypeDesc,CreatedBy,DateCreated,LastUpdatedBy,DateUpdated")] JobType jobType)
        {
            if (id != jobType.JobTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobTypeExists(jobType.JobTypeId))
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
            return PartialView(jobType);
        }
 
        // GET: JobType/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            var jobType = await _context.JobType
                .FirstOrDefaultAsync(m => m.JobTypeId == id);
            if (jobType == null)
            {
                return NotFound(); 
            }

            return PartialView(jobType);
        }

        // POST: JobType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var jobType = await _context.JobType.FindAsync(id);
            _context.JobType.Remove(jobType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobTypeExists(long id)
        {
            return _context.JobType.Any(e => e.JobTypeId == id);
        }
    }
}
