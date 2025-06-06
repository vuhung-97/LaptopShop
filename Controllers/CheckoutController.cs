using LaptopShop.Data;
using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Mail;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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
                lstCart = JsonSerializer.Deserialize<List<CartViewModel>>(ListCart);
            }    

            HttpContext.Session.Set(DsTenKey.ORDER_KEY, lstCart);
            HttpContext.Session.Remove(DsTenKey.CART_KEY); // Xóa giỏ hàng sau khi đặt hàng thành công

            var result = new CheckoutViewModel() { GioHang = lstCart, ThongTinKhachHang = model };

            ViewBag.Time = DateTime.Now.ToString("dd/MM/yyyy - hh/mm/ss");

            int count = 0;
            string iddonhang="";

            using(var db = new ShopLaptopContext())
            {
                count = db.DonHangs.Count();
                do
                {
                    iddonhang = "DH" + (count + 1).ToString("0000");
                    var dh = db.DonHangs.FirstOrDefault(t => t.IdDonHang == iddonhang);
                    if (dh != null)
                        count++;
                    else break;
                }
                while (true);
            }

            var donhang = new DonHang
            {
                IdDonHang = iddonhang,
                
                NgayDat = DateTime.Now,
                DiaChiGiao = model.Ho + " " + model.Ten + "/" + model.SoDienThoai+ "/" +  model.DiaChi + ", " + model.PhuongXa + ", " + model.QuanHuyen + ", " + model.TinhThanh,
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
                    var laptop = db.Laptops.FirstOrDefault(p => p.IdLaptop == item.Id);
                    if(laptop != null)
                    {
                        laptop.SoLuong -= item.Amount;
                    }
                }
                db.SaveChanges();
            }
            TempData["Success"] = "Đặt hàng thành công! Thông tin đơn hàng của bạn đã được ghi nhận. Chúng tôi sẽ liên hệ với bạn trong thời gian sớm nhất.";
            SendContact(result, iddonhang);
            ViewBag.madonhang = iddonhang;
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

        [HttpPost]
        public void SendContact(CheckoutViewModel model, string madonhang)
        {
            
            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("khoaphcn109@gmail.com", "LaptopShop");
            mailMessage.To.Add(model.ThongTinKhachHang.Email);
            mailMessage.Subject = "Thông tin đơn hàng";
            double tonggiatri = 0;
            string dshang = "";
            foreach(var item in model.GioHang)
            {
                tonggiatri += item.ThanhTien;
                dshang += "\n\t+ " + item.Name + "; Số lượng: " + item.Amount + "; Tổng giá: " + item.ThanhTien + "VNĐ";
            }
            string message = "- Xin chào quý khách, thông tin của quý khách:" +
                             "\n\t+ Họ tên khách hàng: " + model.ThongTinKhachHang.Ho + " " + model.ThongTinKhachHang.Ten +
                             "\n\t+ SĐT: " + model.ThongTinKhachHang.SoDienThoai +
                             "\n\t+ Địa chỉ: " + model.ThongTinKhachHang.DiaChi + ", " + model.ThongTinKhachHang.PhuongXa + ", " + model.ThongTinKhachHang.QuanHuyen + ", " + model.ThongTinKhachHang.TinhThanh +
                             "\n- Thông tin đơn hàng:" +
                             "\n\t+ Mã đơn hàng: " + madonhang +
                             "\n\t+ Tổng giá trị đơn hàng: " + tonggiatri +
                             "\n\t+ Thời gian giao dàng dự kiến: 4-5 ngày kể từ ngày đặt hàng" +
                             "\n\t+ Phương thức thanh toán: Thanh toán khi nhận hàng" +
                             "\n\t+ Phí vận chuyển: Miễn phí" +
                             "\n- Danh sách hàng đã đặt:";
            message += dshang;
            mailMessage.Body = $"Tên: {model.ThongTinKhachHang.Ho + " " + model.ThongTinKhachHang.Ten}\nEmail: {model.ThongTinKhachHang.Email}\nNội dung:\n{message}";
            try
            {
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("khoaphcn109@gmail.com", "qnkk hvak obhg ndhv");
                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                }
            }
            catch (Exception ex)
            {
                
            }
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
