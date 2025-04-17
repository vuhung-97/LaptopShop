namespace LaptopShop.ViewModels
{
    public class HomeValueViewModel
    {
        public IEnumerable<LaptopViewModel> LaptopList { get; set; } = null!;
        public IEnumerable<ThuongHieuViewModel> ThuongHieuList { get; set; } = null!;
        public IEnumerable<LoaiViewModel> LoaiList { get; set; } = null!;
    }
}
