using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class UserVoucher
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public Guid VoucherId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Voucher Voucher { get; set; } = null!;
}
