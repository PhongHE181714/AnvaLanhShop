using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Cart
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public Guid? ProductSizeId { get; set; }

    public virtual ProductSize? ProductSize { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
