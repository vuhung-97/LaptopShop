using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopShop.Data;

public partial class ChiTietDonHang
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string IdChiTiet { get; set; } = null!;

    public string IdDonHang { get; set; } = null!;

    public string? IdLaptop { get; set; }

    public string? TenLaptop { get; set; }

    public string? ThuongHieu { get; set; }

    public int? SoLuong { get; set; }

    public double? DonGia { get; set; }

    public virtual DonHang IdDonHangNavigation { get; set; } = null!;
}
