using System.ComponentModel.DataAnnotations;
namespace GreenShop.Models;
public partial class Tag
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string? Description { get; set; }

    public bool Status { get; set; } = true;
}
