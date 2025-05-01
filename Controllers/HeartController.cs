using LaptopShop.Data;
using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Controllers
{
    public class HeartController : Controller
    {
        private readonly ShopLaptopContext db;
        public HeartController(ShopLaptopContext context) => db = context;

        public List<CartViewModel> lstHeart => HttpContext.Session.Get<List<CartViewModel>>(DsTenKey.lIST_HEART_KEY) ?? new List<CartViewModel>();
        public IActionResult Index()
        {
            return View(lstHeart);
        }
        public IActionResult AddToListHeart(string idlaptop, int quantity = 1)
        {
            var danhsach = lstHeart;
            var item = danhsach.SingleOrDefault(it => it.Id == idlaptop);
            if (item == null)
            {
                var temp = db.Laptops.SingleOrDefault(p => p.IdLaptop == idlaptop);
                if (temp == null)
                {
                    item = new CartViewModel();
                    return Redirect("/404");
                }
                else
                {
                    item = new CartViewModel()
                    {
                        Id = temp.IdLaptop,
                        Name = temp.TenLapTop,
                        Hinh = temp.HinhAnh ?? "",
                        Price = (double)(temp.GiaBan ?? 0)
                    };
                    danhsach.Add(item);
                }
            }

            HttpContext.Session.Set(DsTenKey.lIST_HEART_KEY, danhsach);

            return RedirectToAction("Index", "Shop");
        }
        public IActionResult AddToListHeartInHome(string idlaptop, int quantity = 1)
        {
            var danhsach = lstHeart;
            var item = danhsach.SingleOrDefault(it => it.Id == idlaptop);
            if (item == null)
            {
                var temp = db.Laptops.SingleOrDefault(p => p.IdLaptop == idlaptop);
                if (temp == null)
                {
                    item = new CartViewModel();
                    return Redirect("/404");
                }
                else
                {
                    item = new CartViewModel()
                    {
                        Id = temp.IdLaptop,
                        Name = temp.TenLapTop,
                        Hinh = temp.HinhAnh ?? "",
                        Price = temp.GiaBan ?? 0
                    };
                    danhsach.Add(item);
                }
            }

            HttpContext.Session.Set(DsTenKey.lIST_HEART_KEY, danhsach);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromListHeart(string idlaptop)
        {
            var danhsach = lstHeart;
            for (int i = 0; i < danhsach.Count(); i++)
            {
                if (danhsach[i].Id == idlaptop)
                {
                    danhsach.RemoveAt(i);
                    break;
                }
            }

            HttpContext.Session.Set(DsTenKey.lIST_HEART_KEY, danhsach);

            return RedirectToAction("Index");
        }
    }
}
