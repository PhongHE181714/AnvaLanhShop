using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string? Username { get; set; }

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? GoogleId { get; set; }

    public string? Dob { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Token { get; set; }

    public Guid RoleId { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<ShipmentDetail> ShipmentDetails { get; set; } = new List<ShipmentDetail>();

    public virtual ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();
}
