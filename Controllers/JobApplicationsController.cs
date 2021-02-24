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
using Microsoft.Extensions.Logging;
using MimeKit;
using Enum = System.Enum;

namespace learner_portal.Controllers
{
    [Authorize]
    public class JobApplicationsController : BaseController
    {
        private readonly LearnerContext _context;
        private readonly  ILookUpService _lookUpService;
        private readonly ILogger<JobApplicationsController> _logger;
        private readonly EmailConfiguration _emailConfig; 
        private readonly IEmailSender _emailSender;
        private readonly INotyfService _notyf;

        public JobApplicationsController(LearnerContext context,ILookUpService lookUpService, ILogger<JobApplicationsController> logger,EmailConfiguration emailConfig,IEmailSender emailSender, INotyfService notyf)
        {
            _context = context;
            _lookUpService = lookUpService;
            _logger = logger; 
            _emailConfig = emailConfig;
            _emailSender = emailSender;
            _notyf = notyf;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index()
        {
            var data =  await _lookUpService.GetJobApplicationsDetails();
            
            return View( data); 
        }
        
         
        public async Task<JsonResult> GetAllJobApplications()
        {
            try
            {
             //   var draw = HttpContext.Request.Query["draw"].FirstOrDefault();
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
      
                var  listOfJobApplications = await _lookUpService.GetJobApplicationsDetails();

                //total number of rows count
                var recordsTotal = listOfJobApplications.Count();
                //Paging
                var dataList = listOfJobApplications.Skip(skip).Take(pageSize).ToList();
                //Returning Json Data  
                return Json(new   
                    { draw = 0, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = listOfJobApplications.ToList() });
            } 
            catch (Exception)
            {
                throw; 
            } 
        }
        

        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplications = await  _lookUpService.GetJobApplicantPersonByNationalId(id);
            
               
            if (jobApplications == null)
            {
                return NotFound();
            }

            return PartialView(jobApplications);
        }

                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(long id)
        {
            //Get current user details 
            var user = await _lookUpService.GetCurrentLoggedInUser(User.Identity.Name);
            
            //Get currect leaner details
            var learner = await _lookUpService.GetLearnerDetailsByIdEmail(user.Email);
            
            if (ModelState.IsValid)
            {
                //Prepare and application
                var jobApplications = new JobApplications
                {
                    ApplicationStatus = Const.PENDINNG_STATUS,
                    DateApplied = DateTime.Now,
                    LearnerId = learner.LearnerId,
                    JobId = id,
                    
                };

               var lnr=  _context.Learner.FirstOrDefault(l => l.LearnerId == learner.LearnerId);
               lnr.AppliedYn = Const.TRUE;

               await SendAcknowledgementMail(learner);

               _context.Update(lnr);
                _context.Add(jobApplications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); 
            }
            return RedirectToAction("Details","Person", new {id = learner.NationalID});
        }
              
        
        public async Task<IActionResult> Recruited(long id)
        {
            //Get currect leaner details
            var learner = await _lookUpService.GetLearnerDetailsById(id).ConfigureAwait(false);
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobTitle");   
            return PartialView(learner);
        }
     
        [HttpPost, ActionName("Confirm")]

        public async Task<IActionResult> ConfirmRecruited(long id)
        {
             
            var learner =  _context.Learner.FirstOrDefault(a => a.LearnerId == id);

            if (learner != null && learner.AppliedYn.Equals("Yes"))
            {
                learner.RecruitedYn = Const.TRUE;
          

                //  await SendConfirmRequitmentMail(learner);

                _context.Update(learner);
                await _context.SaveChangesAsync();
            }
            else
            {
                _notyf.Error("Please make sure you recruit Applied learners", 5);
               // return RedirectToAction("Index","Learners").WithSuccess("Not Recruited","Please make sure you recruit Applied learners");
            }

            _notyf.Information("Learner will now be placed with a company", 5);  
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobTitle");   
            return RedirectToAction("Index","Learners").WithSuccess("Recruited","Learner will now be placed with a company");
                
        }

          
        private async Task<Mail> SendAcknowledgementMail(LearnerDetailsDto user) 
        
        {
            _logger.LogDebug("Prepare a download CV link");
            var toMail = new MailboxAddress(user.Email);
            var toList = new List<InternetAddress> {toMail};
                
            _logger.LogDebug("Getting Email Template from the DB...");
            var registrationEmailTemplate =
               await _context.EmailTemplates.FirstOrDefaultAsync(e => e.Name == Const.JOB_APPLICATION);
            
            //Check if the template was retrieved successfully
            if (registrationEmailTemplate != null)
            {
                var body = registrationEmailTemplate.EmailBody;

              _logger.LogInformation("Update the Url within the Email Body ...");

                body = body.Replace("##firstname##", user.FirstName);
                body = body.Replace("##lastname##", user.LastName);
                body = body.Replace("##company##", user.CompanyName);
                /*body = body.Replace("##position##", user.);
                body = body.Replace("##expirydate##", user.);*/
                

                _logger.LogInformation("Creating a Mail object ..." + body);
                var message = new Mail
                {
                    FromEmail = _emailConfig.From,
                    To = toList,
                    ToEmail = user.Email,
                    Subject = registrationEmailTemplate.Subject,
                    Body = body,
                    DateCreated = DateTime.Now,
                    CreatedBy =  User.Identity.Name,
                    DateUpdated = DateTime.Now,
                    LastUpdateBy =  User.Identity.Name,
                    
                }; 

                _notyf.Information("Send an email...", 5);

                _emailSender.SendEmail(message);
                return message;
            }

            _logger.LogError("Sending an email failed...");
            return null;
        }
       

        // GET: JobApplications/Create
        public IActionResult Create()
        {
            ViewData["PersonId"] = new SelectList(_context.Person, "PersonId", "PersonId");
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobId");
            return PartialView();
        } 

        // POST: JobApplications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobApplicationId,DateApplied,ApplicationStatus,PersonId,JobId")] JobApplications jobApplications)
        {
            if (ModelState.IsValid)
            { 
                _context.Add(jobApplications);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            

            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobId", jobApplications.JobId);
            
            return PartialView(jobApplications);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobApplications = await _context.JobApplications.FindAsync(id);
            if (jobApplications == null)
            {
                return NotFound();
            }
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobId");
            return PartialView(jobApplications);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid? id, [Bind("JobApplicationId,DateApplied,ApplicationStatus,PersonId,JobId")] JobApplications jobApplications)
        {
            if (id != jobApplications.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplications);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                { 
                    if (!JobApplicationsExists(jobApplications.Id))
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
            ViewData["JobId"] = new SelectList(_context.Jobs, "JobId", "JobId", jobApplications.JobId);
            return PartialView(jobApplications);
        }

        // GET: JobApplications/Delete/5 
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) 
            {
                return NotFound();
            }

            var jobApplications = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplications == null)
            {
                return NotFound();
            }

            return PartialView(jobApplications);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid? id)
        {
            var jobApplications = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(jobApplications);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationsExists(Guid? id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
