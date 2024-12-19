namespace FurniNest_Backend.Models
{
    public class Cart
    {


        public int Id { get; set; }

        public int UserId { get; set; }


        public virtual User? User { get; set; }

        public virtual ICollection<CartItem>? CartItems { get; set; }

        

       
    }
}
