using Microsoft.EntityFrameworkCore;

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


    // Define entities
    
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public bool IsAdmin { get; set; }
    }

    public class InventoryItem
    {
        public int ItemId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public decimal Price { get; set; }
        public string PrimaryPhotoUrl { get; set; } = "";
        public string Category { get; set; } = "";
        public bool IsSold { get; set; }

        public List<ItemImage> Images { get; set; } = new();
        
        public List<SaleItem> SaleItems { get; set; } = new();
        public List<CartItem> CartItems { get; set; } = new();
    }

    public class ItemImage
    {
        public int ImageId { get; set; }
        public int ItemId { get; set; }
        public string ImageUrl { get; set; } = "";
        public int DisplayOrder { get; set; }

        public InventoryItem InventoryItem { get; set; } = null!;
    }

    public class Sale
    {
        public int SaleId { get; set; }
        public int UserId { get; set; }
        
        public string SaleDateTime { get; set; } = "";
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal Total { get; set; }
        public string ShippingSpeed { get; set; } = "";
        public string Street1 { get; set; } = "";
        public string? Street2 { get; set; }
        public string City { get; set; } = "";
        public string State { get; set; } = "";
        public string Zip { get; set; } = "";
        public string CardLast4 { get; set; } = "";

        public User User { get; set; } = null!;
        public List<SaleItem> Items { get; set; } = new();
    }

    public class SaleItem
    {
        public int SaleId { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public Sale Sale { get; set; } = null!;
        public InventoryItem InventoryItem { get; set; } = null!;
    }

    public class Cart
    {
        public int CartId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; } = null!;
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public int CartId { get; set; }
        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public Cart Cart { get; set; } = null!;
        public InventoryItem InventoryItem { get; set; } = null!;
    }
}
