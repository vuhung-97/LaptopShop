using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaptopShop.Data;
using X.PagedList;
using X.PagedList.Extensions;
using LaptopShop.Areas.Model;

namespace LaptopShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ShopLaptopContext _context;

        public HomeController(ShopLaptopContext context)
        {
            _context = context;
        }
        public IActionResult Index(DateTime? fromDate, DateTime? toDate)
        {

            var now = DateTime.Now;
            var firstDayOfMonth = new DateTime(now.Year, now.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //biểu đồ
            fromDate ??= firstDayOfMonth;
            toDate ??= lastDayOfMonth;

            // Đảm bảo không chọn khoảng thời gian ngược
            if (fromDate > toDate)
            {
                var temp = fromDate;
                fromDate = toDate;
                toDate = temp;
            }

            // === Biểu đồ thống kê theo ngày ===
            var ngayHienThi = new List<string>();
            var soLuongHoaDon = new List<int>();
            var tongTienTheoNgay = new List<decimal>();

            for (var date = fromDate.Value.Date; date <= toDate.Value.Date; date = date.AddDays(1))
            {
                var nextDay = date.AddDays(1);

                var hoaDonTrongNgay = _context.DonHangs
                    .Where(d => d.TrangThai == "dagiao" && d.NgayDat >= date && d.NgayDat < nextDay);

                ngayHienThi.Add(date.ToString("dd/MM"));
                soLuongHoaDon.Add(hoaDonTrongNgay.Count());
                tongTienTheoNgay.Add((decimal)(hoaDonTrongNgay.Sum(d => d.TongTien) ?? 0));
            }

            ViewData["NgayHienThi"] = ngayHienThi;
            ViewData["SoDonHang"] = soLuongHoaDon;
            ViewData["TongTien"] = tongTienTheoNgay;

            ViewBag.TuNgay = fromDate.Value.ToString("yyyy-MM-dd");
            ViewBag.DenNgay = toDate.Value.ToString("yyyy-MM-dd");


            //card trên cùng
            var tongSanPham = _context.Laptops
     .Select(l => l.TenLapTop)
     .Distinct()
     .Count();

            // Số loại còn hàng (theo tên, có số lượng > 0)
            var soLuongConHang = _context.Laptops
                .Where(l => l.SoLuong > 0)
                .Select(l => l.TenLapTop)
                .Distinct()
                .Count();

            // Số loại hết hàng (số lượng == 0)
            var soLuongHetHang = _context.Laptops
                .Where(l => l.SoLuong == 0)
                .Select(l => l.TenLapTop)
                .Distinct()
                .Count();

            // Tổng số lượng laptop (tính tổng tất cả tồn kho)
            var tongSoLuong = _context.Laptops
                .Sum(l => l.SoLuong) ?? 0;

            // Các trạng thái đơn hàng cần hiển thị đầy đủ
            var cacTrangThai = new[] { "choxacnhan", "danggiao", "dagiao", "dahuy" };

            // Tạo danh sách đơn hàng theo trạng thái, luôn hiện cả khi = 0
            var donHangTheoTrangThai = cacTrangThai
                .Select(tt => new ThongKeDonGian
                {
                    Ten = tt,
                    SoLuong = _context.DonHangs.Count(d => d.TrangThai == tt && d.NgayDat >= fromDate.Value.Date && d.NgayDat <= toDate.Value.Date)
                })
                .ToList();

            var tongDonHang = donHangTheoTrangThai.Sum(d => d.SoLuong);

            // Doanh thu đã giao
            var tongDoanhThu = _context.DonHangs
                .Where(d => d.TrangThai == "dagiao")
                .Sum(d => d.TongTien) ?? 0;

            // Doanh thu tháng hiện tại
            var doanhThuThang = _context.DonHangs
                .Where(d => d.TrangThai == "dagiao" && d.NgayDat >= fromDate.Value.Date && d.NgayDat <= toDate.Value.Date)
                .Sum(d => d.TongTien) ?? 0;

            // Tài khoản theo vai trò
            var taiKhoanTheoVaiTro = new[]
            {
        new ThongKeDonGian
        {
            Ten = "Admin",
            SoLuong = _context.TaiKhoans.Count(t => t.Loai == "Admin")
        },
        new ThongKeDonGian
        {
            Ten = "KhachHang",
            SoLuong = _context.TaiKhoans.Count(t => t.Loai == "KhachHang")
        }
    }.ToList();

            var tongTaiKhoan = taiKhoanTheoVaiTro.Sum(t => t.SoLuong);



            //tính toán laptop bán chạy
            var laptopBanChays = _context.DonHangs
             .Where(d => d.TrangThai == "dagiao" && d.NgayDat >= fromDate && d.NgayDat <= toDate)
             .SelectMany(d => d.ChiTietDonHangs)
             .Where(ct => !string.IsNullOrEmpty(ct.TenLaptop))
             .GroupBy(ct => ct.TenLaptop)
             .Select(group => new LaptopBanChay
             {
                 Ten = group.Key,
                 SoLuong = group.Sum(g => g.SoLuong) ?? 0,
                 DoanhThu = (double)(group.Sum(g => g.SoLuong * g.DonGia) ?? 0)
             })
             .OrderByDescending(x => x.SoLuong)
             .Take(5)
             .ToList();

            // Kiểm tra nếu không có laptop bán chạy
            if (!laptopBanChays.Any())
            {
                ViewBag.NoLaptopSales = "Không có laptop nào bán được trong khoảng thời gian này.";
            }


            // Cập nhật thông tin laptop bán chạy vào ViewModel

            var viewModel = new Thongke
            {
                TongSanPham = tongSanPham,
                TongSoLuongLaptop = tongSoLuong,
                SoLuongConHang = soLuongConHang,
                SoLuongHetHang = soLuongHetHang,
                TongDonHang = (int)tongDonHang,
                DonHangTheoTrangThai = donHangTheoTrangThai,
                TongDoanhThu = tongDoanhThu,
                DoanhThuThangHienTai = doanhThuThang,
                TongTaiKhoan = (int)tongTaiKhoan,
                TaiKhoanTheoVaiTro = taiKhoanTheoVaiTro,
                LaptopBanChay = laptopBanChays
            };

            return View(viewModel);
        }

    }
}
