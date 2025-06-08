using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LaptopShop.Data;

public partial class Laptop
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? IdLaptop { get; set; } = null!;

    public string? IdThuongHieu { get; set; }

    public string? TenLapTop { get; set; }

    public double? GiaBan { get; set; }

    public string? HinhAnh { get; set; }

    public int? SoLuong { get; set; }

    public string? IdThongTin { get; set; }

    public string? IdLoai { get; set; }


    public virtual Loai? IdLoaiNavigation { get; set; }

    public virtual ThongTinChiTiet? IdThongTinNavigation { get; set; }

    public virtual ThuongHieu? IdThuongHieuNavigation { get; set; }
}
