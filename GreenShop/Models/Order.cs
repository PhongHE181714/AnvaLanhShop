using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Order
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid ShipmentDetailsId { get; set; }

    public DateTime CreateAt { get; set; }

    public float TotalPrice { get; set; }

    public string? Note { get; set; }

    public int Status { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ShipmentDetail ShipmentDetails { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
