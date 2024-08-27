using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Repositories;

namespace EcommerceAPI.Services
{
    public class SellersService
    {
        private readonly SellersRepository _sellersRepository;

        public SellersService(SellersRepository sellersRepository)
        {
            _sellersRepository = sellersRepository;
        }

        public static string ReferenceBuilder(string number)
        {
            var refNumber = "REF" + number.PadLeft(4, '0');
            return refNumber;
        }

        public async Task<bool> CheckSeller(string reference)
        {
            var seller = _sellersRepository.GetByReference(reference);
            if (seller is null) return false;
            return true;
        }
    }
}