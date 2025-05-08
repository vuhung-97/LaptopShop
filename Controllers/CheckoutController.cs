using LaptopShop.Data;
using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
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

        [HttpPost]
        public IActionResult DatHang(CheckoutViewModel Model)
        {
            var model = Model.ThongTinKhachHang;            
            var ListCart = HttpContext.Session.Get(DsTenKey.CART_KEY);
            var lstCart = new List<CartViewModel>();

            if(ListCart != null)
            {
                // Chuyển lại dữ liệu từ byte về List<CartViewModel>
                lstCart = JsonSerializer.Deserialize<List<CartViewModel>>(ListCart);
            }    

            HttpContext.Session.Set(DsTenKey.ORDER_KEY, lstCart);
            HttpContext.Session.Remove(DsTenKey.CART_KEY); // Xóa giỏ hàng sau khi đặt hàng thành công

            var result = new CheckoutViewModel() { GioHang = lstCart, ThongTinKhachHang = model };


            ViewBag.Time = DateTime.Now.ToString("dd/MM/yyyy - hh/mm/ss");

            int count = 0;
            using(var db = new ShopLaptopContext())
            {
                count = db.DonHangs.Count(); 
            }

            var donhang = new DonHang
            {
                IdDonHang = "DH" + (count + 1).ToString("0000"),
                NgayDat = DateTime.Now,
                DiaChiGiao = model.DiaChi + ", " + model.PhuongXa + ", " + model.QuanHuyen + ", " + model.TinhThanh,
                TongTien = result.GioHang.Sum(x => x.ThanhTien),
                TrangThai = "ChoXacNhan"
            };
            using (var db = new ShopLaptopContext())
            {
                db.DonHangs.Add(donhang);
                foreach (var item in lstCart)
                {
                    var chitietdonhang = new ChiTietDonHang
                    {
                        IdDonHang = donhang.IdDonHang,
                        IdLaptop = item.Id,
                        SoLuong = item.Amount,
                        DonGia = item.Price
                    };
                    db.ChiTietDonHangs.Add(chitietdonhang);
                }
                db.SaveChanges();
            }
            TempData["Success"] = "Đặt hàng thành công! Thông tin đơn hàng của bạn đã được ghi nhận. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.";
            return View(result);
        }

        public IActionResult ShowOrder()
        {
            var lstCart = HttpContext.Session.Get<List<CartViewModel>>(DsTenKey.ORDER_KEY);
            if (lstCart == null)
            {
                TempData["Messange"] = "Không có đơn hàng nào!";
                return RedirectToAction("Index");
            }
            return View(lstCart);
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
