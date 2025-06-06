using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

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
