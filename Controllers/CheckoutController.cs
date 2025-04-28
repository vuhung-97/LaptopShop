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

            var ListCart = HttpContext.Session.Get(DsTenKey.CART_KEY);
            var lstCart = new List<CartViewModel>();

            if(ListCart != null)
            {
                // Chuyển lại dữ liệu từ byte về List<CartViewModel>
                lstCart = JsonSerializer.Deserialize<List<CartViewModel>>(ListCart);
            }    

            var result = new CheckoutViewModel() { GioHang = lstCart, ThongTinKhachHang = model };

            HttpContext.Session.Set<List<CartViewModel>>(DsTenKey.ORDER_KEY, lstCart);
            HttpContext.Session.Remove(DsTenKey.CART_KEY);

            ViewBag.Time = DateTime.Now.ToString("dd/MM/yyyy - hh/mm/ss");

            return View(result); // hoặc trả về trang cảm ơn
        }
        public IActionResult ShowHoaDon()
        {
            return View();
        }
        public async Task<IActionResult> GetTinhThanh()
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync("https://provinces.open-api.vn/api/p/");
                return Content(response, "application/json");
            }
        }

        public async Task<IActionResult> GetQuanHuyen(string tinhThanhCode)
        {
            using (var httpClient = new HttpClient())
            {
                // Sử dụng đúng API để lấy quận huyện theo mã tỉnh thành
                var response = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/p/{tinhThanhCode}?depth=2");
                return Content(response, "application/json");
            }
        }

        public async Task<IActionResult> GetPhuongXa(string quanHuyenCode)
        {
            using (var httpClient = new HttpClient())
            {
                // Sử dụng đúng API để lấy phường xã theo mã quận huyện
                var response = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/d/{quanHuyenCode}?depth=2");
                return Content(response, "application/json");
            }
        }
    }
}
