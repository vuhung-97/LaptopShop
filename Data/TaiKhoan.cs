using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class TaiKhoan
{
    public string IdTaiKhoan { get; set; } = null!;

    public string? HoTen { get; set; }

    public string? MatKhau { get; set; }

    public string? Loai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();
}
