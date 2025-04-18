namespace LaptopShop.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartViewModel> GioHang { get; set; } = new();
        public OrderInfoViewModel ThongTinKhachHang { get; set; } = new();
    }

}
