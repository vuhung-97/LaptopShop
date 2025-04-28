using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class ThuongHieu
{
    public string IdThuongHieu { get; set; } = null!;

    public string? TenThuongHieu { get; set; }

    public string? Logo { get; set; }

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}
