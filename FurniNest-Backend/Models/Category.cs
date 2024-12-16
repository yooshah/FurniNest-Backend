namespace FurniNest_Backend.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
