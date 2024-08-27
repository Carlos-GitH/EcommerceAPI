using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.DTOs;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class ClientsService
    {
        private readonly ShoppingCartsRepository _shoppingcartRepository;
        private readonly ClientsRepository _clientsRepository;

        public ClientsService(ShoppingCartsRepository shoppingcartRepository, ClientsRepository clientsRepository)
        {
            _shoppingcartRepository = shoppingcartRepository;
            _clientsRepository = clientsRepository;
        }

        public async Task<CreatedShoppingCartDTO> CreateShoppingCart(int clientId)
        {
            var clientShoppingCart = new CreateShoppingCartDTO
            {
                client_id = clientId
            };
            var shoppingCart = await _shoppingcartRepository.CreateShoppingCart(clientShoppingCart);
            return shoppingCart;
        }

        public async Task<bool> CheckClient(string cpf_cnpj)
        {
            var client = await _clientsRepository.GetByCpfCnpj(cpf_cnpj);
            if(client is null) return false;
            return true;
        }

        public bool ValidateProprietaryClient(string client_id, int idUrl)
        {
            var strIdUrl = idUrl.ToString();
            if(strIdUrl != client_id) return false;
            return true;
        }
    }
}