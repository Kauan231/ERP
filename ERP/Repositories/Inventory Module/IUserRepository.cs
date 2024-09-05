using ERP.Data.Dtos;
using ERP.Models;

namespace ERP.Repositories
{
    public interface IUserRepository
    {
        void SaveChanges();
        User Create(CreateUserDto userDto);
        void Delete(string id);
    }
}
