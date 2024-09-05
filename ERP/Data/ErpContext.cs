using Microsoft.EntityFrameworkCore;
using ERP.Models;

namespace ERP.Data
{
    public class ErpContext : DbContext
    {
        public ErpContext(DbContextOptions<ErpContext> opts)
        : base(opts)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .HasOne(product => product.Inventories)
                .WithMany(inventory => inventory.Products)
                .HasForeignKey(product => product.inventoryId);
            builder.Entity<Inventory>()
                .HasOne(inventory => inventory.Users)
                .WithMany(user => user.Inventories)
                .HasForeignKey(inventory => inventory.userId);
            builder.Entity<Shipment>()
                .HasOne(shipment => shipment.Products)
                .WithMany(product => product.shipments)
                .HasForeignKey(shipment => shipment.productId);

        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
