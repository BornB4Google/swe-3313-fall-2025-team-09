using Microsoft.EntityFrameworkCore;
using backend.models;

namespace Backend.Data
{
    public class StorefrontDbContext : DbContext
    {
        public StorefrontDbContext(DbContextOptions<StorefrontDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<InventoryItem> InventoryItems { get; set; } = null!;
        public DbSet<ItemImage> ItemImages { get; set; } = null!;
        public DbSet<Sale> Sales { get; set; } = null!;
        public DbSet<SaleItem> SaleItems { get; set; } = null!;
        public DbSet<Cart> Carts { get; set; } = null!;
        public DbSet<CartItem> CartItems { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Explicit primary keys
            modelBuilder.Entity<InventoryItem>()
                .HasKey(i => i.ItemId);

            modelBuilder.Entity<ItemImage>()
                .HasKey(img => img.ImageId);

            // User unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // InventoryItem → ItemImage (1-to-many)
            modelBuilder.Entity<ItemImage>()
                .HasOne(img => img.InventoryItem)
                .WithMany(item => item.Images)
                .HasForeignKey(img => img.ItemId);

            // SaleItem composite key
            modelBuilder.Entity<SaleItem>()
                .HasKey(si => new { si.SaleId, si.ItemId });

            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.Sale)
                .WithMany(s => s.Items)
                .HasForeignKey(si => si.SaleId);

            // many SaleItems per InventoryItem
            modelBuilder.Entity<SaleItem>()
                .HasOne(si => si.InventoryItem)
                .WithMany(i => i.SaleItems)
                .HasForeignKey(si => si.ItemId);

            // CartItem composite key
            modelBuilder.Entity<CartItem>()
                .HasKey(ci => new { ci.CartId, ci.ItemId });

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId);

            // many CartItems per InventoryItem
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.InventoryItem)
                .WithMany(i => i.CartItems)
                .HasForeignKey(ci => ci.ItemId);

            // Cart → User (many carts per user.. can be checked out or active)
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            // Sale → User (many sales per user)
            modelBuilder.Entity<Sale>()
                .HasOne(s => s.User)
                .WithMany()
                .HasForeignKey(s => s.UserId);
        }
    }


   
