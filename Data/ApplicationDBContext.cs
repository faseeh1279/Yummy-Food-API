using Microsoft.EntityFrameworkCore;

namespace Yummy_Food_API
{
    public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<ItemCategory> ItemCategories { get; set; }

        // Relationships 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.ItemCategory)
            .WithMany(c => c.Items)
            .HasForeignKey(m => m.ItemCateogryID); 
        }
        
    }
}