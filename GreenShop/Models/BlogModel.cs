namespace GreenShop.Models
{
    public class BlogModel
    {

        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Img { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Author { get; set; } = null!;
        public string Description { get; set; } = null!;
       

    }
}
