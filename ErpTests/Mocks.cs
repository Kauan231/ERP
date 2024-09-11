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

        public static Product TestProduct(string InventoryID, int initialAmount)
        {
            Product product = new Product();  
            product.Id = Guid.NewGuid().ToString(); 
            product.Name = Guid.NewGuid().ToString();
            product.Description = Guid.NewGuid().ToString();
            product.inventoryId = InventoryID;
            product.Amount = initialAmount;
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
    }
}
