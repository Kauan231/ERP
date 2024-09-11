using ERP.Models;
using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos.Domain
{
    public class CreateBusinessDto
    {
        public string Name { get; set; }
        public string? userId { get; set; }
    }
}
