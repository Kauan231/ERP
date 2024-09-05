using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlTypes;
using System.Linq;

namespace ERP.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public UserRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public User Create(CreateUserDto createDto)
        {
            User user = _mapper.Map<User>(createDto);
            var guid = Guid.NewGuid().ToString();
            user.Id = guid;
            _context.Users.Add(user);
            return user;
        }
        public void Delete(string id)
        {
            User user = _context.Users.SingleOrDefault(x => x.Id == id);
            if(user != null)
            {
                _context.Users.Remove(user);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
