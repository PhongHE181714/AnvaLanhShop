using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public int Status { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
