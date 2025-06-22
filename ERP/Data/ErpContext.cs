using Microsoft.EntityFrameworkCore;
using ERP.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ERP.Models.Domain;
using ERP.Migrations;

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
            builder.Entity<InventoryItem>()
                .HasOne(inventoryItem => inventoryItem.Inventory)
                .WithMany(inventory => inventory.InventoryItems)
                .HasForeignKey(inventoryItem => inventoryItem.inventoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Business>()
                .HasOne(business => business.Users)
                .WithMany(user => user.Businesses)
                .HasForeignKey(business => business.userId);
            builder.Entity<Inventory>()
                .HasOne(inventory => inventory.Businesses)
                .WithMany(business => business.Inventories)
                .HasForeignKey(inventory => inventory.businessId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Shipment>()
                .HasMany(shipment => shipment.OrderItems)
                .WithOne(orderItem => orderItem.Shipment)
                .HasForeignKey(orderItem => orderItem.shipmentId);
            builder.Entity<Order>()
                .HasOne(order => order.client)
                .WithMany(client => client.Orders)
                .HasForeignKey(order => order.clientId);
            builder.Entity<Order>()
                .HasOne(order => order.Shipment)
                .WithMany(shipment => shipment.Orders)
                .HasForeignKey(order => order.shipmentId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Order>()
                .HasMany(o => o.InventoryItems)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderInventoryItems"));
            builder.Entity<InventoryItem>()
                .HasOne(InventoryItem => InventoryItem.Product)
                .WithMany(product => product.InventoryItems)
                .HasForeignKey(InventoryItem => InventoryItem.productId);
            builder.Entity<Product>()
                .HasOne(product => product.Business)
                .WithMany(business => business.Products)
                .HasForeignKey(product => product.businessId);
            builder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.Product)
                .WithMany(product => product.OrderItems)
                .HasForeignKey(orderItem => orderItem.productId);
            builder.Entity<Shipment>()
                .HasOne(shipment => shipment.Inventory)
                .WithMany(inventory => inventory.Shipments)
                .HasForeignKey(shipment => shipment.inventoryId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<OrderItem>()
                .HasOne(orderItem => orderItem.InventoryItem)
                .WithMany()
                .HasForeignKey(orderItem => orderItem.inventoryItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<InventoryItem> InventoryItems { get; set; }
    }
}
