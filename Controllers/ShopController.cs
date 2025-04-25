using LaptopShop.Data;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace LaptopShop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopLaptopContext db;

        public ShopController(ShopLaptopContext context) => db = context;
        public IActionResult Index(string? idloai, string? idthuonghieu, int? idkhoanggia = null)
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

            double? minPrice = null;
            double? maxPrice = null;
            if (idkhoanggia.HasValue)
            {
                var priceFilterVM = new PriceFilterViewModel();
                var selectedRange = priceFilterVM.PriceRanges.FirstOrDefault(r => r.Id == idkhoanggia.Value);
                if (selectedRange != null)
                {
                    minPrice = selectedRange.MinPrice;
                    maxPrice = selectedRange.MaxPrice;
                    laptops = laptops.Where(p => p.GiaBan >= minPrice && (maxPrice == null || p.GiaBan <= maxPrice));
                }
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
            }).OrderByDescending(p => p.GiaBan).ToList();

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
                TenLoai = p.IdLoaiNavigation.TenLoai,
                Cpu = p.IdThongTinNavigation.Cpu,
                Ram = p.IdThongTinNavigation.Ram,
                Ocung = p.IdThongTinNavigation.Ocung,
                ManHinh = p.IdThongTinNavigation.ManHinh
            }).ToList();

            return View("Index" , result);
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
        public IActionResult SortProduct(string sortOrder)
        {
            var lap = db.Laptops.Select(p => new LaptopViewModel
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
            });

            // Sắp xếp theo sortOrder
            switch (sortOrder)
            {
                case "asc":
                    lap = lap.OrderBy(p => p.GiaBan);
                    break;
                case "desc":
                    lap = lap.OrderByDescending(p => p.GiaBan);
                    break;
                default:
                    break;
            }

            return PartialView("LaptopItem", lap.ToList());
        }
    }
}
