using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class TaiKhoan
{
    public string IdTaiKhoan { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? DienThoai { get; set; }

    public string? DiaChi { get; set; }

    public string? Loai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
