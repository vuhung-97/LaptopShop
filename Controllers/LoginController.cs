using Microsoft.AspNetCore.Mvc;
using LaptopShop.Data;
using LaptopShop.Helpers;

namespace MyShopApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ShopLaptopContext d;
        public LoginController(ShopLaptopContext context) => d = context;
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckAccount(string us, string qw)
        {
            var acc_data = d.TaiKhoans.SingleOrDefault(p => p.IdTaiKhoan == us && p.MatKhau == qw);
            //if (acc_data == null) 
            //{
            //    return View("Index");
            //}

            HttpContext.Session.SetString(DsTenKey.USER_NAME_KEY, us);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // hoặc .Remove("Tên session") nếu muốn xóa session cụ thể
            return RedirectToAction("Index", "Home");
        }

    }
}
