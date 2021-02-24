using System.Diagnostics;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using learner_portal.Models;
using learner_portal.Services;
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc.Rendering;

namespace learner_portal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<Roles> _roleManager;
        private readonly ILookUpService _lookUpService;
        private readonly IToastifyService _toastify;
        public HomeController( RoleManager<Roles> roleManager,ILogger<HomeController> logger,ILookUpService lookUpService,IToastifyService toastify)
        {
            _roleManager = roleManager;
            _logger = logger;
            _lookUpService = lookUpService;
            _toastify = toastify;
        }

        public IActionResult Index()
        {
            _toastify.Error("Users already exists");
            var jobs = _lookUpService.GetAllJob().Result;
            
            ViewData["Name"] = new SelectList(_roleManager.Roles, "Name", "Name");
            return View( jobs); 
        }
        
        /*
        public IActionResult _AvailableJobs()
        {
            var jobs = _lookUpService.GetAllJob();
  
            return PartialView(jobs); 
        }*/
        [HttpGet]
        public IActionResult _AvailableJobs()
        {
            var jobs = _lookUpService.GetAllJob().Result;
  
            return PartialView(jobs); 
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}