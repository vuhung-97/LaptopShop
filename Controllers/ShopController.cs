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
                TenLoai = p.IdLoaiNavigation.TenLoai,
                Cpu = p.IdThongTinNavigation.Cpu,
                Ram = p.IdThongTinNavigation.Ram,
                Ocung = p.IdThongTinNavigation.Ocung,
                ManHinh = p.IdThongTinNavigation.ManHinh
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
            var lt = db.Laptops.Include(p => p.IdThuongHieuNavigation)
                                   .Include(p => p.IdLoaiNavigation)
                                   .SingleOrDefault(p => p.IdLaptop == idlaptop);
            var d = db.ThongTinChiTiets.SingleOrDefault(p => p.IdThongTin == lt.IdThongTin);

            var laptop = new LaptopViewModel
            {
                IdLaptop = lt.IdLaptop,
                TenLapTop = lt.TenLapTop,
                GiaBan = lt.GiaBan,
                HinhAnh = lt.HinhAnh,
                ThuongHieu = lt.IdThuongHieuNavigation.TenThuongHieu,
                TenLoai = lt.IdLoaiNavigation.TenLoai
            };

            var detail = new DetailViewModel
            {
                IdThongTin = d.IdThongTin,
                Ram = d.Ram,
                Ocung = d.Ocung,
                ManHinh = d.ManHinh,
                Pin = d.Pin,
                DoHoa = d.DoHoa,
                Cpu = d.Cpu,
                CongGiaoTiep = d.CongGiaoTiep,
                HeDieuHanh = d.HeDieuHanh,
                TrongLuong = d.TrongLuong
            };

            var laptopDetail = new LaptopDetailViewModel
            {
                Laptop = laptop,
                Detail = detail
            };

            ViewBag.sl = soluong;

            return View("Detail", laptopDetail);
        }
    }
}
