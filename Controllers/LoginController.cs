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

        public IActionResult CheckAccount(string us, string pw)
        {
            var acc_data = d.TaiKhoans.SingleOrDefault(p => p.IdTaiKhoan == us && p.MatKhau == pw);

            if (acc_data == null)

            {
                ViewBag.ThongBao = "Sai tài khoản hoặc mật khẩu.";
                return View("Index");
            }    


            HttpContext.Session.SetString(DsTenKey.USER_NAME_KEY, acc_data.HoTen);
            // phân quyền
            HttpContext.Session.SetString("Role", acc_data.Loai);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // hoặc .Remove("Tên session") nếu muốn xóa session cụ thể
            return RedirectToAction("Index", "Home");
        }

        public async Task<string> LayDiaChiAsync(string DiaChi, string TinhThanh, string QuanHuyen, string PhuongXa)
        {
            using (var httpClient = new HttpClient())
            {
                
                // Lấy tên Tỉnh/Thành
                if (TinhThanh == null)
                {
                    return DiaChi;
                }    
                var tinhResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/p/{TinhThanh}");
                dynamic tinhData = Newtonsoft.Json.JsonConvert.DeserializeObject(tinhResponse);
                string tenTinh = tinhData.name;

                // Lấy tên Quận/Huyện
                if (QuanHuyen == null)
                {
                    return DiaChi + ", " + tenTinh;
                }
                var huyenResponse = await httpClient.GetStringAsync($"https://provinces.open-api.vn/api/d/{QuanHuyen}?depth=2");
                dynamic huyenData = Newtonsoft.Json.JsonConvert.DeserializeObject(huyenResponse);
                string tenHuyen = huyenData.name;

                // Lấy tên Phường/Xã
                string tenXa = "";
                if(PhuongXa == null)
                {
                    return DiaChi + ", " + tenHuyen + ", " + tenTinh;
                }
                foreach (var ward in huyenData.wards)
                {
                    if (ward.code == PhuongXa)
                    {
                        tenXa = ward.name;
                        break;
                    }
                }

                // Tạo địa chỉ đầy đủ
                string diaChiDayDu = $"{DiaChi}, {tenXa}, {tenHuyen}, {tenTinh}";

                return diaChiDayDu;
            }
        }

        public IActionResult DangKyTaiKhoan(string IdTaiKhoan, string HoTen, string MatKhau, string? Email, string? DienThoai, string? TinhThanh, string? QuanHuyen, string? PhuongXa, string? DiaChi)
        {
            using (var db = new ShopLaptopContext())
            {
                var tkcheck = db.TaiKhoans.FirstOrDefault(p => p.IdTaiKhoan == IdTaiKhoan);
                if(tkcheck != null)
                {
                    ViewBag.thongbao = "Tài khoản đã tồn tại.";
                    return View("DangKy");
                }    
            }    

            string diaChiDayDu = LayDiaChiAsync(DiaChi, TinhThanh, QuanHuyen, PhuongXa).Result;

            var tk = new LaptopShop.Data.TaiKhoan()
            {
                IdTaiKhoan = IdTaiKhoan,
                HoTen = HoTen,
                MatKhau = MatKhau,
                Email = Email,
                DienThoai = DienThoai,
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
