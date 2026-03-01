using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Image { get; set; } = null!;

    public float Rate { get; set; }

    public string Description { get; set; } = null!;

    public virtual Category? Category { get; set; }

    public Guid CategoryId { get; set; }

    public int Status { get; set; }


    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

    public virtual ICollection<ProductSize> ProductSizes { get; set; } = new List<ProductSize>();
}
