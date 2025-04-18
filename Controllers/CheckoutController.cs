using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace LaptopShop.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            var lstCart = HttpContext.Session.Get<List<CartViewModel>>(DsTenKey.CART_KEY);
            if (lstCart == null) 
                lstCart = new List<CartViewModel>();
            var info = new OrderInfoViewModel();
            var checkoutInfor = new CheckoutViewModel
            {
                GioHang = lstCart,
                ThongTinKhachHang = info
            };
            return View(checkoutInfor);
        }

        public IActionResult debug()
        {
            if (TempData["Errors"] != null)
            {
                var json = TempData["Errors"].ToString();
                var lst = JsonSerializer.Deserialize<List<string>>(json);
                return View(lst);
            }

            return View(new List<string>());
        }


        [HttpPost]
        public IActionResult DatHang(CheckoutViewModel Model)
        {
            // Kiểm tra model nếu cần
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

                TempData["Errors"] = JsonSerializer.Serialize(errors); // cần using System.Text.Json
                return RedirectToAction("debug", errors); // hoặc view hiện tại
            }

            var model = Model.ThongTinKhachHang;

            // Gom chuỗi thông tin
            string fullInfo = $"Họ tên: {model.Ho} {model.Ten}\n" +
                              $"Email: {model.Email}\n" +
                              $"SĐT: {model.SoDienThoai}\n" +
                              $"Địa chỉ: {model.DiaChi}, {model.PhuongXa}, {model.QuanHuyen}, {model.TinhThanh}\n" +
                              $"Mã bưu điện: {model.MaBuuDien}\n" +
                              $"Phương thức thanh toán: {model.PhuongThucThanhToan}";

            // Ví dụ: lưu vào cơ sở dữ liệu hoặc ghi log

            var lstCart = Model.GioHang;

            var result = new CheckoutViewModel() { GioHang = lstCart, ThongTinKhachHang = model };

            ViewBag.Time = DateTime.Now.ToString("dd/MM/yyyy - hh/mm/ss");

            return View(result); // hoặc trả về trang cảm ơn
        }

    }
}
