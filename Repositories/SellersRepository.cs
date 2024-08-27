using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class SellersRepository
    {
        private readonly AppDbContext _dbContext;

        public SellersRepository(AppDbContext dBcontext)
        {
            _dbContext = dBcontext;
        }

        public async Task<IEnumerable<Seller>> GetAll()
        {
            return await _dbContext.Sellers.ToListAsync();
        }

        public async Task<Seller> GetById(int id)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(s => s.id == id);
        }

        public async Task<Seller> Create(Seller seller)
        {
            _dbContext.Sellers.Add(seller);
            await _dbContext.SaveChangesAsync();
            return seller;
        }

        public async Task<Seller> GetByEmail(string email)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(s => s.email == email);
        }

        public async Task<Seller> GetByReference(string reference)
        {
            return await _dbContext.Sellers.FirstOrDefaultAsync(s => s.reference == reference);
        }

        public async Task<Seller> Update(Seller seller)
        {
            _dbContext.Sellers.Update(seller);
            await _dbContext.SaveChangesAsync();
            return seller;
        }

        public async Task Delete(Seller seller)
        {
            _dbContext.Sellers.Remove(seller);
            await _dbContext.SaveChangesAsync();
        }
    }
}