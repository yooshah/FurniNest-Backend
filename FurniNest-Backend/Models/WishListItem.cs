using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.Models
{
    public class WishListItem
    {

        [Required]
        public int Id { get; set; }

        [Required]
        public int WishListId { get; set; }

        [Required]
        public int ProductId { get; set; }

        public virtual Product? Product { get; set; }

        public virtual  WishList? WishList { get; set; }


       

    }
}
