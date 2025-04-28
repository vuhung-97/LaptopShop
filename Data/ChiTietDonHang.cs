using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class ChiTietDonHang
{
    public string IdDonHang { get; set; } = null!;

    public string IdLaptop { get; set; } = null!;

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public virtual DonHang IdDonHangNavigation { get; set; } = null!;

    public virtual Laptop IdLaptopNavigation { get; set; } = null!;
}
