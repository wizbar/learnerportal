using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using ExcelDataReader;
using learner_portal.DTO;
using learner_portal.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace learner_portal.Controllers
{
    [Authorize]
    public class DataImportController : BaseController
    {
        private readonly LearnerContext _learnerContext;
        private readonly ILookUpService _lookUpService;
        private readonly IFileService _fileService;        
        private readonly IDataImportService _dataService;
        private readonly UserManager<Users> _userManager;
        private readonly FoldersConfigation _foldersConfigation;        
        private readonly INotyfService _notyf;

        public DataImportController(ILookUpService lookUpService,IFileService fileService, IDataImportService dataService, FoldersConfigation foldersConfigation,INotyfService notyf,UserManager<Users> userManager,LearnerContext learnerContext)
        {
            _userManager = userManager;
            _fileService = fileService;
            _foldersConfigation = foldersConfigation;
            _lookUpService = lookUpService;
            _notyf = notyf;
            _learnerContext = learnerContext;
            _dataService = dataService;
        }

        // GET: Cities
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> ImportData()
        {
            var user = await  _userManager.Users.FirstOrDefaultAsync(a => a.UserName.Equals(User.Identity.Name));
            var userRole = await _userManager.GetRolesAsync(user);
            ViewData["DocumentTypeId"] = new SelectList(  await _lookUpService.GetDocumentTypesDetailsByRole(userRole[0]), "Id", "TypeName");
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ImportData(FileDTO fileDto)
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
                //Upload the Excel Spreadsheet of learners
                var path =  _fileService.UploadFile(fileDto.File, _foldersConfigation.Uploads);
                
                //import the data
                if(path != null){
                    await _dataService.ImportExcelForLearners(path);
                    _notyf.Success("File uploaded successfully...");
                }
                else
                {
                    _notyf.Error("Invalid file uploaded");
                    return Json(new { Result = "ERROR", Message = "Invalid file uploaded"});
                }
                return Json(new { Result = "OK" });
            }
            catch (Exception ex)
            {
                _notyf.Error("Something went wrong : " + ex.Message );
                return Json(new { Result = "ERROR", Message = ex.Message});
            }
        }
 
    }
}
