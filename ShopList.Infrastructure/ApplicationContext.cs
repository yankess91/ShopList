using Microsoft.EntityFrameworkCore;
using ShopList.Infrastructure.Model;
using System.Reflection;

namespace ShopList.Infrastructure.Database
{
    public class ApplicationContext : DbContext

    {
        public DbSet<Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=shoppingList.db", options =>
            {
                options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
               .Property(e => e.Id)
               .ValueGeneratedOnAdd();

            modelBuilder.Entity<ShoppingList>(b =>
            {
                b.HasIndex(e => e.Name)
                .IsUnique();
                b.Property(e => e.Id)
                .ValueGeneratedOnAdd();
                b.HasMany(p => p.ProductList)
                   .WithOne(s => s.ShoppingList)
                   .HasForeignKey(s => s.ShoppingListId);
            });
        }
    }
}