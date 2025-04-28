using ERP.Models;
using ERP.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class CreateShipmentDto
    {   
        public List<OrderItem> OrderItems { get; set; }
    }
}
