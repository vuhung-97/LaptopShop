using LaptopShop.Data;

namespace LaptopShop.Areas.Model
{
    public class LaptopEditViewModel
    {
        public Laptop? Laptop { get; set; }
        public ThongTinChiTiet? ThongTin { get; set; }

       
      

        // Dùng để upload ảnh mới
        public List<IFormFile>? HinhAnhMoi { get; set; }

        // Ảnh cũ giữ lại sau khi xoá trên client
        public List<string>? OldImages { get; set; }
    }


}
