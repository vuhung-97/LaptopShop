﻿using LaptopShop.Data;
using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShopLaptopContext db;

        public ShoppingCartController(ShopLaptopContext context) => db = context;

        public List<CartViewModel> lstCart => HttpContext.Session.Get<List<CartViewModel>>(DsTenKey.CART_KEY) ?? new List<CartViewModel> ();

        public IActionResult Index()
        {
            return View(lstCart);
        }

        public IActionResult AddToCart(string idlaptop, int soluong = 1)
        {
            Add(idlaptop, soluong);
            return RedirectToAction("Index", "Shop");
        }


        public IActionResult AddToCartInHome(string idlaptop, int soluong = 1)
        {
            Add(idlaptop, soluong);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AddToCartAndShow(string idlaptop, int soluong = 1)
        {
            Add(idlaptop, soluong);
            return RedirectToAction("Index");
        }

        public void Add(string idlaptop, int soluong = 1)
        {
            var giohang = lstCart;
            var laptop = giohang.SingleOrDefault(p => p.Id == idlaptop);

            if(laptop == null)
            {
                var temp = db.Laptops.SingleOrDefault (p => p.IdLaptop == idlaptop);
                if (temp != null)
                {
                    CartViewModel cart = new CartViewModel
                    {
                        Id = temp.IdLaptop,
                        Name = temp.TenLapTop,
                        Hinh = temp.HinhAnh?.Split(",").FirstOrDefault(),
                        Price = temp.GiaBan ?? 0,
                        Amount = soluong,
                        Quantity = temp.SoLuong ?? 0
                    };
                    giohang.Add(cart);
                }
            }
            else
            {
                laptop.Amount += soluong;
            }
            HttpContext.Session.Set(DsTenKey.CART_KEY, giohang);
        }
    
        public IActionResult Remove(string idlaptop)
        {
            var giohang = lstCart;
            
            for(int i=0; i<giohang.Count(); i++)
            {
                if (giohang[i].Id == idlaptop)
                {
                    giohang.RemoveAt(i);
                    break;
                }
            }
            HttpContext.Session.Set(DsTenKey.CART_KEY, giohang);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(string idlaptop, int soluong)
        {
            if (soluong < 1) soluong = 1;

            var cart = HttpContext.Session.Get<List<CartViewModel>>(DsTenKey.CART_KEY);
            var item = cart?.FirstOrDefault(x => x.Id == idlaptop);
            if (item != null)
            {
                item.Amount = soluong;
                //item.ThanhTien = item.Price * soluong;
            }
            HttpContext.Session.Set(DsTenKey.CART_KEY, cart);

            return RedirectToAction("Index");
        }

    }
}
