using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class LoginService
    {
        private readonly ClientsRepository _clientsRepository;
        private readonly SellersRepository _sellersRepository;

        public LoginService(ClientsRepository clientsRepository, SellersRepository sellersRepository)
        {
            _clientsRepository = clientsRepository;
            _sellersRepository = sellersRepository;
        }

        public async Task<string> LoginClient(string email, string password)
        {
            var client = await _clientsRepository.GetByEmail(email);
            if(client == null)
            {
                return "Client not found";
            }
            if(client.password != password)
            {
                return "Invalid password";
            }
            return "Authorized";
        }

        public async Task<string> LoginSeller(string email, string password)
        {
            var seller = await _sellersRepository.GetByEmail(email);
            if(seller == null)
            {
                return "Seller not found";
            }
            if(seller.password != password)
            {
                return "Invalid password";
            }
            return "Authorized";
        }
    }
}