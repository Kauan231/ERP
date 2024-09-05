using ERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErpTests
{
    public static class Mocks
    {
        public static User TestUser()
        {
            User user = new User();
            user.Id = Guid.NewGuid().ToString();
            user.Name = Guid.NewGuid().ToString();
            return user;
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

        public static Inventory TestInventory(string userID)
        {
            Inventory inventory = new Inventory();
            inventory.Id = Guid.NewGuid().ToString();
            inventory.userId = userID;
            return inventory;
        }
    }
}
