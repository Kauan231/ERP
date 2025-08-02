using ERP.Models;
using ERP.Models.Domain;

namespace ErpTests
{
    public static class Mocks
    {
        public static User TestUser()
        {
            User user = new User();
            user.UserName = Guid.NewGuid().ToString();
            return user;
        }

        public static Business TestBusiness(string userID)
        {
            Business business = new Business();
            business.Id = Guid.NewGuid().ToString();
            business.userId = userID;
            business.Name = Guid.NewGuid().ToString();
            return business;
        }

        public static Product TestProduct(string businessId)
        {
            Product product = new Product();
            product.Id = Guid.NewGuid().ToString();
            product.Name = Guid.NewGuid().ToString();
            product.Description = Guid.NewGuid().ToString();
            product.businessId = businessId;
            return product;
        }

        public static Inventory TestInventory(string businessID)
        {
            Inventory inventory = new Inventory();
            inventory.Id = Guid.NewGuid().ToString();
            inventory.businessId = businessID;
            inventory.Name = Guid.NewGuid().ToString();
            return inventory;
        }

        public static InventoryItem TestInventoryItem(string productId, int amount, string inventoryId)
        {
            InventoryItem inventoryItem = new InventoryItem();
            inventoryItem.Id = Guid.NewGuid().ToString();
            inventoryItem.productId = productId;
            inventoryItem.inventoryId = inventoryId;
            inventoryItem.Amount = amount;
            return inventoryItem;
        }
    }
}
