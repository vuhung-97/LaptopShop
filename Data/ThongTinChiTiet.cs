using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class ThongTinChiTiet
{
    public string IdThongTin { get; set; } = null!;

    public string? Cpu { get; set; }

    public string? Ram { get; set; }

    public string? Ocung { get; set; }

    public string? ManHinh { get; set; }

    public string? DoHoa { get; set; }

    public string? CongGiaoTiep { get; set; }

    public string? HeDieuHanh { get; set; }

    public string? Pin { get; set; }

    public string? TrongLuong { get; set; }

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}
