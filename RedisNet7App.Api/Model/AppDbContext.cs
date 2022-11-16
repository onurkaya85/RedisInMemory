using Microsoft.EntityFrameworkCore;

namespace RedisNet7App.Api.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                   new Product()
                   {
                       Id = 1,
                       Name = "Kalem",
                       Price = 5
                   },
                   new Product()
                   {
                       Id = 2,
                       Name = "Kitap",
                       Price = 15
                   },
                   new Product()
                   {
                       Id = 3,
                       Name = "Defter",
                       Price = 25
                   });
            base.OnModelCreating(modelBuilder);
        }
    }
}
