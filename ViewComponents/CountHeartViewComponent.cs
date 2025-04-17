using LaptopShop.Helpers;
using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace LaptopShop.ViewComponents
{
    public class CountHeartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var value = HttpContext.Session.GetString(DsTenKey.lIST_HEART_KEY);
            List<CartViewModel> result = new List<CartViewModel>();

            if (value != null)
            {
                result = JsonSerializer.Deserialize<List<CartViewModel>>(value);
            }

            return View(result);
        }
    }
}
