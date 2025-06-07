using LaptopShop.Data;

namespace LaptopShop.Areas.Model
{
    public class DonHangTam
    {
        public string IdDonHang { get; set; }
        public string HoTenNguoiNhan { get; set; }
        public string SoDienThoaiNguoiNhan { get; set; }
        public string EmailNguoiNhan { get; set; }
        public string DiaChiNguoiNhan { get; set; }
        public DateTime NgayDat { get; set; }
        public double? TongTien { get; set; }
        public string TrangThai { get; set; }
        public List<ChiTietDonHang> ChiTietDonHangs { get; set; }
    }
}