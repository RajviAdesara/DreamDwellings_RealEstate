using Microsoft.AspNetCore.Mvc;

namespace RealEstate.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
