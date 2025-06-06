using LaptopShop.Data;

namespace LaptopShop.ViewModels
{
    public class LaptopViewModel
    {
        public string IdLaptop { get; set; } = null!;

        public string? ThuongHieu { get; set; }

        public string? TenLapTop { get; set; }

        public double GiaBan { get; set; }

        public int SoLuong { get; set; }

        public string? HinhAnh { get; set; }

        public string? TenLoai { get; set; }

        public string? Cpu { get; set; }

        public string? Ram { get; set; }

        public string? Ocung { get; set; }

        public string? ManHinh { get; set; }
    }
}
