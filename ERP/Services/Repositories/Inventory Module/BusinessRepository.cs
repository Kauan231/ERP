using AutoMapper;
using ERP.Data;
using ERP.Data.Dtos;
using ERP.Data.Dtos.Domain;
using ERP.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ERP.Repositories
{
    public class BusinessRepository : IBusinessRepository
    {
        private readonly ErpContext _context;
        private readonly IMapper _mapper;
        public BusinessRepository(ErpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public Business Create(CreateBusinessDto createDto)
        {
            Business business = _mapper.Map<Business>(createDto);
            var guid = Guid.NewGuid().ToString();
            business.Id = guid;
            _context.Businesses.Add(business);
            return business;
        }

        public ReadBusinessDto Read(string businessId)
        {
            Business business = _context.Businesses.Include(x => x.Inventories).SingleOrDefault(x => x.Id == businessId);
            ReadBusinessDto readBusinessDto = _mapper.Map<ReadBusinessDto>(business);
            return readBusinessDto;
        }

        public List<Business> ReadAllUserBusinesses(string userId) {
            List<Business> readBusinessDto = _context.Businesses.Where(x => x.userId == userId).ToList();
            return readBusinessDto;
        }

        public void Delete(string id)
        {
            Business business = _context.Businesses.SingleOrDefault(x => x.Id == id);
            if(business != null)
            {
                _context.Businesses.Remove(business);
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
