using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class DonHang
{
    public string IdDonHang { get; set; } = null!;

    public string? IdTaiKhoan { get; set; }

    public DateTime NgayDat { get; set; }

    public string? DiaChiGiao { get; set; }

    public double? TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }
}
