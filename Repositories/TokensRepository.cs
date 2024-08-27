using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class TokensRepository
    {
        private readonly AppDbContext _dbContext;

        public TokensRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Token> Create(Token token)
        {
            _dbContext.Tokens.Add(token);
            await _dbContext.SaveChangesAsync();

            return token;
        }

        public async Task<Token?> GetByTokenString(string token)
        {
            return await _dbContext.Tokens.FirstOrDefaultAsync(t => t.token == token);
        }

        public async Task DeleteByTokenString(string token)
        {
            var tokenToDelete = await _dbContext.Tokens.FirstAsync(t => t.token == token);
            _dbContext.Tokens.Remove(tokenToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}