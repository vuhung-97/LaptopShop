using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
