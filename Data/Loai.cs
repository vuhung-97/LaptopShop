using System;
using System.Collections.Generic;

namespace LaptopShop.Data;

public partial class Loai
{
    public string IdLoai { get; set; } = null!;

    public string TenLoai { get; set; }

    public virtual ICollection<Laptop> Laptops { get; set; } = new List<Laptop>();
}
