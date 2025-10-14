using System;
using System.Collections.Generic;

namespace GreenShop.Models;

public partial class News
{
    public Guid Id { get; set; }

    public string Title { get; set; } = null!;

    public string Img { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int View { get; set; }

    public string Author { get; set; } = null!;

    public string Source { get; set; } = null!;

    public int Status { get; set; }

    public Guid UserId { get; set; }

    public virtual ICollection<NewDetail> NewDetails { get; set; } = new List<NewDetail>();

    public virtual User User { get; set; } = null!;
}
