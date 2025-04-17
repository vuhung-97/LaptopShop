using LaptopShop.Data;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly ShopLaptopContext db;
        public MenuLoaiViewComponent(ShopLaptopContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke(string? idLoai)
        {
            var loai = db.Loais.Select(p => new LoaiViewModel
            {
                IdLoai = p.IdLoai,
                TenLoai = p.TenLoai,
                SoLuong = p.Laptops.Count
            }).ToList();
            ViewBag.CheckedLoai = idLoai;
            return View(loai);
        }
    }
}
