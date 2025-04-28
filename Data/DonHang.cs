using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class DonHang
{
    public int IdDonHang { get; set; }

    public string? IdTaiKhoan { get; set; }

    public DateTime? NgayDatHang { get; set; }

    public double? TongTien { get; set; }

    public DateTime? NgayCapNhat { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }
}
