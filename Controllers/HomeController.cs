using System.Diagnostics;
using LaptopShop.Data;
using LaptopShop.Models;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopLaptopContext db;

        public HomeController(ILogger<HomeController> logger, ShopLaptopContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            var lap = db.Laptops.Select(p => new LaptopViewModel
            {
                IdLaptop = p.IdLaptop,
                TenLapTop = p.TenLapTop,
                GiaBan = p.GiaBan,
                HinhAnh = p.HinhAnh,
                ThuongHieu = p.IdThuongHieuNavigation.TenThuongHieu,
                TenLoai = p.IdLoaiNavigation.TenLoai
            }).Take(6).OrderBy(p => p.TenLoai);

            var loai = db.Loais.Select(p => new LoaiViewModel
            {
                IdLoai = p.IdLoai,
                TenLoai = p.TenLoai,
                SoLuong = p.Laptops.Count
            });

            var thuonghieu = db.ThuongHieus.Select(p => new ThuongHieuViewModel
            {
                IdThuongHieu = p.IdThuongHieu,
                TenThuongHieu = p.TenThuongHieu,
                Logo = p.Logo
            });

            var data = new HomeValueViewModel
            {
                LaptopList = lap.ToList(),
                LoaiList = loai.ToList(),
                ThuongHieuList = thuonghieu.ToList()
            };

            return View(data);
        }

        public IActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
