using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Domain.Context
{
    public class AmitDbContext : IdentityDbContext
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
                .HasForeignKey(x => x.OrderId);
            modelBuilder.Entity<UserChosenTextile>()
                .HasKey(t => new {t.UserId, t.TextileId});
            modelBuilder.Entity<UserChosenTextile>()
                .HasOne(x => x.Textile)
                .WithMany(x => x.UserChosenTextiles)
                .HasForeignKey(x => x.TextileId);
            modelBuilder.Entity<UserChosenTextile>()
                .HasOne(x => x.User)
                .WithMany(x => x.UserChosenTextiles)
                .HasForeignKey(x => x.UserId);



        }
        public DbSet<FilterCharachteristics> FilterCharachteristicses { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Charachteristic> Charachteristics { get; set; }

        public DbSet<CharachteristicValues> CharachteristicValues { get; set; }

        public DbSet<CharachteristicVariants> CharachteristicVariants { get; set; }

        public DbSet<ChildCategory> ChildCategories { get; set; }

        public DbSet<ChildCommentQuestion> ChildCommentQuestions { get; set; }

        public DbSet<ChildCommentReview> ChildCommentReviews { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<ParentCommentQuestion> ParentCommentQuestions { get; set; }

        public DbSet<ParentCommentReview> ParentCommentReviews { get; set; }

        public DbSet<Textile> Textiles { get; set; }

        public DbSet<User> Users { get; set; }



    }
}