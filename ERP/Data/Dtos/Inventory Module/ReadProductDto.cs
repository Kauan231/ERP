using ERP.Models;

namespace ERP.Data.Dtos
{
    public class ReadProductDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string businessId { get; set; }
    }

    public class ReadProductTableDto
    {
        public List<Product> AllProducts { get; set; }
        public int totalOfItems { get; set; }
    }
}
