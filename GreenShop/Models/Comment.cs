using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class Comment
{
    public Guid Id { get; set; }

    public float Rate { get; set; }

    public string Content { get; set; } = null!;

    public DateTime CommentedAt { get; set; }

    public int Status { get; set; }

    public Guid ProductId { get; set; }

    public Guid UserId { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
