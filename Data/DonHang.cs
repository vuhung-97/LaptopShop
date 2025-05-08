using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LaptopShop.Data;

public partial class DonHang
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string IdDonHang { get; set; } = null!;

    public string? IdTaiKhoan { get; set; }

    public DateTime NgayDat { get; set; }

    public string? DiaChiGiao { get; set; }

    public double? TongTien { get; set; }

    public string TrangThai { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual TaiKhoan? IdTaiKhoanNavigation { get; set; }
}
