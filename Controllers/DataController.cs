using Microsoft.AspNetCore.Mvc;

namespace learner_portal.Controllers
{
    public class DataController : BaseController
    {
        // GET
        public IActionResult DataImport()
        {
            return View();
        }
    }
}