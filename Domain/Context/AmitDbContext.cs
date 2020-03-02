using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Domain.Context
{
    public class AmitDbContext : DbContext
    {
        public AmitDbContext(DbContextOptions options) :base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ItemOrder>()
                .HasKey(t => new { t.ItemId, t.OrderId });
            modelBuilder.Entity<ItemOrder>()
                .HasOne(x => x.Item)
                .WithMany(t => t.ItemOrders)
                .HasForeignKey(x => x.ItemId);
            modelBuilder.Entity<ItemOrder>()
                .HasOne(x => x.Order)
                .WithMany(t => t.ItemOrders )
                .HasForeignKey(x => x.Order);



        }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Charachteristic> Charachteristics { get; set; }

        

    }
}