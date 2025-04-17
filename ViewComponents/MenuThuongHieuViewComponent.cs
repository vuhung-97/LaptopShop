using LaptopShop.Data;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.ViewComponents
{
    public class MenuThuongHieuViewComponent : ViewComponent
    {
        private readonly ShopLaptopContext db;

        public MenuThuongHieuViewComponent(ShopLaptopContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke(string? idThuongHieu)
        {
            var thuonghieu = db.ThuongHieus.Select(p => new ThuongHieuViewModel
            {
                IdThuongHieu = p.IdThuongHieu,
                TenThuongHieu = p.TenThuongHieu,
                Logo = p.Logo,
                SoLuong = p.Laptops.Count
            }).ToList();
        
            ViewBag.CheckedThuongHieu = idThuongHieu;
            return View(thuonghieu);
        }
    }
}
