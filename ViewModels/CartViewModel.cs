namespace LaptopShop.ViewModels
{
    public class CartViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public string? Hinh { get; set; }
        public double ThanhTien => Amount * Price;
    }
}
