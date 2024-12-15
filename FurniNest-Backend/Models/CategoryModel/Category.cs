
using FurniNest_Backend.Models.ProductModel;

namespace FurniNest_Backend.Models.CategoryModel
{
    public class Category
    {
        public int CategoryId { get; set; }

        public string? Name { get; set; }

        public virtual ICollection<Product>? Products { get; set; }
    }
}
