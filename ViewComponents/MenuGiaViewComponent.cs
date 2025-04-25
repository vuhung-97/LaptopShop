using LaptopShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LaptopShop.ViewComponents
{
    public class MenuGiaViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int? selectedPriceRange)
        {
            var priceFilterVM = new PriceFilterViewModel
            {
                SelectedRangeId = selectedPriceRange
            };

            // Truyền giá trị đã chọn vào ViewBag để hiển thị trong view
            ViewBag.CheckedPrice = selectedPriceRange;

            return View(priceFilterVM);
        }
    }
}
