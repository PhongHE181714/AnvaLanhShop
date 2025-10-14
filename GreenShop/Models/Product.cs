using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public float Price { get; set; }

    public int Quantity { get; set; }

    public string Image { get; set; } = null!;

    public float Rate { get; set; }

    public string Description { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
}
