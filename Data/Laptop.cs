using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class Laptop
{
    public string IdLaptop { get; set; } = null!;

    public string IdThuongHieu { get; set; }

    public string TenLapTop { get; set; }

    public double GiaBan { get; set; }

    public string? HinhAnh { get; set; }

    public string IdThongTin { get; set; }

    public string IdLoai { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual Loai IdLoaiNavigation { get; set; }

    public virtual ThongTinChiTiet? IdThongTinNavigation { get; set; }

    public virtual ThuongHieu IdThuongHieuNavigation { get; set; }
}
