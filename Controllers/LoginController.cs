using Microsoft.AspNetCore.Mvc;
using LaptopShop.Data;
using LaptopShop.Helpers;
using LaptopShop.ViewModels;

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

            if (acc_data == null)

            {
                ViewBag.ThongBao = "Sai tài khoản hoặc mật khẩu.";
                return View("Index");
            }    


            HttpContext.Session.SetString(DsTenKey.USER_NAME_KEY, us);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // hoặc .Remove("Tên session") nếu muốn xóa session cụ thể
            return RedirectToAction("Index", "Home");
        }

        public async Task<string> LayDiaChiAsync(LaptopShop.ViewModels.TaiKhoan taiKhoan)
        {
            using (var httpClient = new HttpClient())
            {
                // Lấy tên Tỉnh/Thành
                var tinhResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/p/{taiKhoan.TinhThanh}");
                dynamic tinhData = Newtonsoft.Json.JsonConvert.DeserializeObject(tinhResponse);
                string tenTinh = tinhData.name;

                // Lấy tên Quận/Huyện
                var huyenResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/d/{taiKhoan.QuanHuyen}?depth=2");
                dynamic huyenData = Newtonsoft.Json.JsonConvert.DeserializeObject(huyenResponse);
                string tenHuyen = huyenData.name;

                // Lấy tên Phường/Xã
                string tenXa = "";
                foreach (var ward in huyenData.wards)
                {
                    if (ward.code == taiKhoan.PhuongXa)
                    {
                        tenXa = ward.name;
                        break;
                    }
                }

                // Tạo địa chỉ đầy đủ
                string diaChiDayDu = $"{taiKhoan.DiaChi}, {tenXa}, {tenHuyen}, {tenTinh}";

                // TODO: Lưu vào DB

                return diaChiDayDu;
            }
        }

        public IActionResult DangKyTaiKhoan(LaptopShop.ViewModels.TaiKhoan taiKhoan)
        {
            using (var db = new ShopLaptopContext())
            {
                var tkcheck = db.TaiKhoans.FirstOrDefault(p => p.IdTaiKhoan == taiKhoan.IdTaiKhoan);
                if(tkcheck != null)
                {
                    ViewBag.thongbao = "Tài khoản đã tồn tại.";
                    return View("DangKy");
                }    
            }    

            string diaChiDayDu = LayDiaChiAsync(taiKhoan).Result;

            var tk = new LaptopShop.Data.TaiKhoan()
            {
                IdTaiKhoan = taiKhoan.IdTaiKhoan,
                HoTen = taiKhoan.HoTen,
                MatKhau = taiKhoan.MatKhau,
                Email = taiKhoan.Email,
                DienThoai = taiKhoan.DienThoai,
                DiaChi = diaChiDayDu,
                Loai = "KhachHang"
            };

            using(var db = new ShopLaptopContext())
            {
                db.TaiKhoans.Add(tk);
                db.SaveChanges();
            }    

            return View("Index");
        }

        public IActionResult DangKy()
        {
            return View("DangKy");
        }
    }
}
