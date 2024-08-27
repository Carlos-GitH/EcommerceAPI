using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Data;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Repositories
{
    public class ClientsRepository
    {
        private readonly AppDbContext _dbContext;

        public ClientsRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _dbContext.Clients.ToListAsync();
        }

        public async Task<Client> GetById(int id)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Client> GetByCpfCnpj(string cpf_cnpj)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.cpf_cnpj == cpf_cnpj);
        }

        public async Task<Client> GetByEmail(string email)
        {
            return await _dbContext.Clients.FirstOrDefaultAsync(c => c.email == email);
        }

        public async Task<Client> Create(Client client)
        {
            _dbContext.Clients.Add(client);
            await _dbContext.SaveChangesAsync();
            return client;
        }

        public async Task Update(Client client)
        {
            _dbContext.Clients.Update(client);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Client client)
        {
            _dbContext.Clients.Remove(client);
            await _dbContext.SaveChangesAsync();
        }
    }
}