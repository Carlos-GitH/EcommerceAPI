using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class CartItemsService
    {
        private readonly ProductsRepository _productsRepository;

        public CartItemsService(ProductsRepository productsRepository)
        {
            _productsRepository = productsRepository;
        }
    }
}