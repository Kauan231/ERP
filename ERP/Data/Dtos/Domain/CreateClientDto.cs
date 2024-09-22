using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos.Domain
{
    public class CreateClientDto
    {
        public string Name { get; set; }
        public string? businessId { get; set; }
    }
}
