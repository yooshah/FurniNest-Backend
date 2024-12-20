using System.ComponentModel.DataAnnotations;

namespace FurniNest_Backend.Models
{
    public class WishList
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int userId { get; set; }

       
        public virtual User? User {  get; set; }
        
        public virtual ICollection<WishListItem>? WishListItems { get; set; }




    }
}
