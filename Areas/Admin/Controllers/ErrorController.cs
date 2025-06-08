using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ErrorController : Controller
    {
        [Route("Admin/Error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
