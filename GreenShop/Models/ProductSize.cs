using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class ProductSize
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string SizeName { get; set; } = null!;
    public float Price { get; set; }
    public int Quantity { get; set; }

    public virtual Product Product { get; set; } = null!;
}
