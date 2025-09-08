using Microsoft.EntityFrameworkCore;
using Yummy_Food_API.Models.Domain;

namespace Yummy_Food_API
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<CustomerProfile> CustomerProfiles { get; set; }
        public DbSet<AdminProfile> AdminProfiles { get; set; }
        public DbSet<RiderProfile> RiderProfiles { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<ItemImage> ItemImages { get; set; }

        // Relationships 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // One-to-many CustomerProfile -> Complaints
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.CustomerProfile)
                .WithMany(cp => cp.Complaints)
                .HasForeignKey(c => c.CustomerProfileId);

            // One-to-many RiderProfile -> Complaints
            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.RiderProfile)
                .WithMany(rp => rp.Complaints)
                .HasForeignKey(c => c.RiderProfileId);

            // One-to-many Items -> ItemImages 
            modelBuilder.Entity<Item>()
                .HasMany(i => i.Images)          
                .WithOne(it => it.Item)        
                .HasForeignKey(it => it.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // One-to-many ItemCategory -> Items
            modelBuilder.Entity<Item>()
                .HasOne(i => i.ItemCategory)
                .WithMany(ic => ic.Items)
                .HasForeignKey(i => i.ItemCategoryId);

            // One-to-many CustomerProfile -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.CustomerProfile)
                .WithMany(cp => cp.Orders)
                .HasForeignKey(o => o.CustomerProfileId);

            // One-to-many RiderProfile -> Orders
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Rider)
                .WithMany(rp => rp.Orders)
                .HasForeignKey(o => o.RiderProfileId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithOne(u => u.RefreshToken)
                .HasForeignKey<RefreshToken>(rt => rt.UserId);

            modelBuilder
                .Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();
        }
    }
}