using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class ProductCategory
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
