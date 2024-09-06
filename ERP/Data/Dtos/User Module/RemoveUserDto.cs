using System.ComponentModel.DataAnnotations;

namespace ERP.Data.Dtos
{
    public class RemoveUserDto
    {
        [Required]
        public string UserIdentityId { get; set; }
    }
}
