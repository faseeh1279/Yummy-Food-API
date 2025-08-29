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
            //// One-to-one User <-> AdminProfile
            //modelBuilder.Entity<AdminProfile>()
            //    .HasOne(a => a.User)
            //    .WithOne(u => u.AdminProfile)
            //    .HasForeignKey<AdminProfile>(a => a.UserId);

            //// One-to-one User <-> CustomerProfile
            //modelBuilder.Entity<CustomerProfile>()
            //    .HasOne(c => c.User)
            //    .WithOne(u => u.CustomerProfile)
            //    .HasForeignKey<CustomerProfile>(c => c.UserId);

            //// One-to-one User <-> RiderProfile
            //modelBuilder.Entity<RiderProfile>()
            //    .HasOne(r => r.User)
            //    .WithOne(u => u.RiderProfile)
            //    .HasForeignKey<RiderProfile>(r => r.UserId);

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

            //// One-to-one User -> RefreshToken
            //modelBuilder.Entity<RefreshToken>()
            //    .HasOne(rt => rt.User)
            //    .WithOne(u => u.RefreshToken)
            //    .HasForeignKey<RefreshToken>(rt => rt.UserId);

        }
    }
}