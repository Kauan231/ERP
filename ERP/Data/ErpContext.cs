using Microsoft.EntityFrameworkCore;
using ERP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ERP.Models.Domain;

namespace ERP.Data
{
    public class ErpContext : IdentityDbContext<User>
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
            builder.Entity<Business>()
                .HasOne(business => business.Users)
                .WithMany(user => user.Businesses)
                .HasForeignKey(business => business.userId);
            builder.Entity<Inventory>()
                .HasOne(inventory => inventory.Businesses)
                .WithMany(business => business.Inventories)
                .HasForeignKey(inventory => inventory.businessId);
            builder.Entity<Shipment>()
                .HasOne(shipment => shipment.Products)
                .WithMany(product => product.shipments)
                .HasForeignKey(shipment => shipment.productId);

        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
