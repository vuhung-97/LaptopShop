using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
