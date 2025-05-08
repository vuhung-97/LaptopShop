using Microsoft.AspNetCore.Mvc;
using LaptopShop.Data;
using LaptopShop.Helpers;
using NuGet.Protocol.Events;

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

        public IActionResult DangKy()
        {
            return View();
        }

        public IActionResult CheckDangKy(string IdTaiKhoan, string HoTen, string MatKhau, string? Email, string? DienThoai, string? DiaChi, string? TinhThanh, string? QuanHuyen, string? PhuongXa)
        {
            using(var db = new ShopLaptopContext())
            {
                var tk = db.TaiKhoans.FirstOrDefault(p => p.IdTaiKhoan == IdTaiKhoan);
                if(tk != null)
                {
                    ViewBag.ThongBao = "Tài khoản đã tồn tại, vui lòng nhập tài khoản khác.";
                    return View("DangKy");
                }
            }    
            string diaChiDayDu = DiaChi + ", " + LayDiaChiAsync(TinhThanh, QuanHuyen, PhuongXa).Result;
            using(var db = new ShopLaptopContext())
            {
                var taikhoan = new LaptopShop.Data.TaiKhoan()
                {
                    IdTaiKhoan = IdTaiKhoan,
                    HoTen = HoTen,
                    MatKhau = MatKhau,
                    Email = Email,
                    DienThoai = DienThoai,
                    DiaChi = diaChiDayDu,
                    Loai = "KhachHang"
                };
                db.TaiKhoans.Add(taikhoan);
                db.SaveChanges();
            }    
            return View("Index");
        }    

        public async Task<string> LayDiaChiAsync(string TinhThanh, string QuanHuyen, string PhuongXa)
        {          
            using (var httpClient = new HttpClient())
            {
                // Lấy tên Tỉnh/Thành
                if (TinhThanh == null) return "";
                var tinhResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/p/{TinhThanh}");
                dynamic tinhData = Newtonsoft.Json.JsonConvert.DeserializeObject(tinhResponse);
                string tenTinh = tinhData.name;

                // Lấy tên Quận/Huyện
                if (QuanHuyen == null) return tenTinh;
                var huyenResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/d/{QuanHuyen}?depth=2");
                dynamic huyenData = Newtonsoft.Json.JsonConvert.DeserializeObject(huyenResponse);
                string tenHuyen = huyenData.name;

                // Lấy tên Phường/Xã
                string tenXa = "";
                if (PhuongXa == null) return tenHuyen + ", " + tenTinh;
                foreach (var ward in huyenData.wards)
                {
                    if (ward.code == PhuongXa)
                    {
                        tenXa = ward.name;
                        break;
                    }
                }

                // Tạo địa chỉ đầy đủ
                string diaChiDayDu = $"{tenXa}, {tenHuyen}, {tenTinh}";

                // TODO: Lưu vào DB

                return diaChiDayDu;
            }
        }
        public IActionResult CheckAccount(string us, string pw)
        {
            var acc_data = d.TaiKhoans.SingleOrDefault(p => p.IdTaiKhoan == us && p.MatKhau == pw);
            if(acc_data == null)
            {
                ViewBag.ThongBao = "Tên tài khoản hoặc mật khẩu không đúng!";
                return View("Index");
            }    
            HttpContext.Session.SetString(DsTenKey.USER_NAME_KEY, acc_data.HoTen);            

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // hoặc .Remove("Tên session") nếu muốn xóa session cụ thể
            return RedirectToAction("Index", "Home");
        }

    }
}
