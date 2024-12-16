
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
        }


    }
}
