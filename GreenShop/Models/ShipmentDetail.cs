using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class ShipmentDetail
{
    public Guid Id { get; set; }

    public string Address { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string Receiver { get; set; } = null!;

    public Guid UserId { get; set; }

    public int Status { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User User { get; set; } = null!;
}
