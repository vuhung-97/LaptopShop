namespace LaptopShop.Areas.Model
{
    public class Thongke
    {
        // Tổng số loại laptop (tính theo tên, không cộng dồn số lượng)
        public int TongSanPham { get; set; }

        // Tổng số lượng laptop còn trong kho
        public int TongSoLuongLaptop { get; set; }

        // Số lượng laptop còn hàng (> 0)
        public int SoLuongConHang { get; set; }

        // Số lượng laptop hết hàng (= 0)
        public int SoLuongHetHang { get; set; }

        // Tổng số đơn hàng
        public int TongDonHang { get; set; }

        // Danh sách đơn hàng theo trạng thái (VD: "Chờ xác nhận" : 5)
        public List<ThongKeDonGian> DonHangTheoTrangThai { get; set; }

        // Doanh thu
        public double TongDoanhThu { get; set; }
        public double DoanhThuThangHienTai { get; set; }

        // Tổng tài khoản
        public int TongTaiKhoan { get; set; }

        // Phân loại tài khoản theo vai trò (VD: "Admin" : 2, "Người dùng" : 20)
        public List<ThongKeDonGian> TaiKhoanTheoVaiTro { get; set; }
        public List<LaptopBanChay> LaptopBanChay { get; set; }
    }

    public class ThongKeDonGian
    {
        public string Ten { get; set; }
        public int? SoLuong { get; set; }
    }
    public class LaptopBanChay
    {
        public string Ten { get; set; }
        public int? SoLuong { get; set; }
        public double DoanhThu { get; set; }
    }


}
