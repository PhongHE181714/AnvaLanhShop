using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class NewDetail
{
    public Guid Id { get; set; }

    public string Content { get; set; } = null!;

    public int Status { get; set; }

    public int Type { get; set; }

    public int ContentIndex { get; set; }

    public Guid NewId { get; set; }

    public virtual News New { get; set; } = null!;
}
