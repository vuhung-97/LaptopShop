using LaptopShop.Data;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LaptopShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopLaptopContext db;

        public ShopController(ShopLaptopContext context) => db = context;
        public IActionResult Index(string? idloai, string? idthuonghieu)
        {
            var laptops = db.Laptops.AsQueryable();

            if (!string.IsNullOrEmpty(idloai))
            {
                laptops = laptops.Where(p => p.IdLoai == idloai);
            }

            if (!string.IsNullOrEmpty(idthuonghieu))
            {
                laptops = laptops.Where(p => p.IdThuongHieu == idthuonghieu);
            }

            if (laptops.Count() == 0)
            {
                TempData["Messange"] = "Không tìm thấy sản phẩm nào!";
                return RedirectToAction("PageNotFound", "Home");
            }

            var lap = laptops.Select(p => new LaptopViewModel
            {
                IdLaptop = p.IdLaptop,
                TenLapTop = p.TenLapTop,
                GiaBan = p.GiaBan,
                HinhAnh = p.HinhAnh,
                ThuongHieu = p.IdThuongHieuNavigation.TenThuongHieu,
                TenLoai = p.IdLoaiNavigation.TenLoai
            }).ToList();

            return View(lap);
        }

        public IActionResult Search(string? query)
        {
            var laptops = db.Laptops.AsQueryable();
            if (!string.IsNullOrEmpty(query))
            {
                laptops = laptops.Where(p => p.TenLapTop.Contains(query) || p.IdThuongHieuNavigation.TenThuongHieu.Contains(query));
            }

            if (laptops.Count() == 0)
            {
                TempData["Messange"] = "Không tìm thấy sản phẩm nào!";
                return RedirectToAction("PageNotFound", "Home");
            }

            var result = laptops.Select(p => new LaptopViewModel
            {
                IdLaptop = p.IdLaptop,
                TenLapTop = p.TenLapTop,
                GiaBan = p.GiaBan,
                HinhAnh = p.HinhAnh,
                ThuongHieu = p.IdThuongHieuNavigation.TenThuongHieu,
                TenLoai = p.IdLoaiNavigation.TenLoai
            }).ToList();

            return View(result);
        }

        public IActionResult Detail(string? idlaptop, int soluong = 1)
        {
            var laptop = db.Laptops.Include(p => p.IdThuongHieuNavigation)
                                   .Include(p => p.IdLoaiNavigation)
                                   .SingleOrDefault(p => p.IdLaptop == idlaptop);
            if (laptop == null)
            {
                TempData["Messange"] = "Không tìm thấy sản phẩm nào!";
                return RedirectToAction("PageNotFound", "Home");
            }

            var laptopDetail = new LaptopViewModel
            {
                IdLaptop = laptop.IdLaptop,
                TenLapTop = laptop.TenLapTop,
                GiaBan = laptop.GiaBan,
                HinhAnh = laptop.HinhAnh,
                ThuongHieu = laptop.IdThuongHieuNavigation.TenThuongHieu,
                TenLoai = laptop.IdLoaiNavigation.TenLoai
            };

            ViewBag.sl = soluong;

            return View("Detail", laptopDetail);
        }
    }
}
