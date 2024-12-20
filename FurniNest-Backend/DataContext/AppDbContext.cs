
using FurniNest_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FurniNest_Backend.DataContext
{
    public class AppDbContext:DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }



        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<WishList> WishLists { get; set; }

        public DbSet<WishListItem> WishListItems { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasKey(x => x.Id);


            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("user");

            modelBuilder.Entity<User>()
                .Property(u => u.AccountStatus)
                .HasDefaultValue(true);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Product>()
                 .Property(p => p.Price)
                 .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Category>().HasData(

                new Category { CategoryId=1,Name= "Sofas" },
                new Category { CategoryId=2,Name= "Living" },
                new Category { CategoryId=3,Name= "Bedroom" },
                new Category { CategoryId=4,Name= "Dining" },
                new Category { CategoryId=5,Name= "New Arrivals" },
                new Category { CategoryId=6,Name= "Office" },
                new Category { CategoryId=7,Name= "Kitchen" }
                );

            //modelBuilder.Entity<Cart>()
            //    .HasOne(u => u.User)
            //    .WithOne(p => p.Cart)
            //    .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .HasOne(r => r.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(k => k.UserId);
                

            modelBuilder.Entity<CartItem>()
                .HasOne(r => r.Cart)
                .WithMany(r => r.CartItems)
                .HasForeignKey(r => r.CartId);

            modelBuilder.Entity<CartItem>()
                .HasOne(r=>r.Product)
                .WithMany(r=>r.CartItems)
                .HasForeignKey(r => r.ProductId);


            modelBuilder.Entity<User>()
                .HasOne(r => r.WishList)
                .WithOne(r => r.User)
                .HasForeignKey<WishList>(k=>k.userId);

            modelBuilder.Entity<WishListItem>()
                 .HasOne(r=>r.WishList)
                 .WithMany(r=>r.WishListItems)
                 .HasForeignKey(k=>k.WishListId);

            modelBuilder.Entity<Product>()
                .HasMany(r => r.WishListItems)
                .WithOne(r => r.Product)
                .HasForeignKey(k=>k.ProductId);
               
                




                

            
                
        }




    }
}
