using LaptopShop.Data;

namespace LaptopShop.Areas.Model
{
    public class LaptopCreateViewModel
    {
        public Laptop? Laptop { get; set; }
        public ThongTinChiTiet? ThongTin { get; set; }
        public List<IFormFile>? HinhAnhMoi { get; set; }
    }

}
