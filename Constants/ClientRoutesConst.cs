using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcommerceAPI.Constants
{
    public class ClientRoutesConst
    {
        public const string client                 = "/api/v1/clients/id";
        public const string personal_info          = "/api/v1/clients/personal_info";
        public const string address                = "/api/v1/clients/address";
        public const string GetByCpfCnpj           = "/api/v1/clients/get_by_cpf_cnpj";
        public const string getByEmail             = "/api/v1/clients/get_by_email";
        public const string editClient             = "/api/v1/clients/edit";
        public const string DeleteClient           = "/api/v1/clients/delete";
        public const string getOrderById           = "/api/v1/orders/id";
        public const string getOrdersByClientId    = "/api/v1/orders/client/id";
        public const string createOrder            = "/api/v1/orders/create";
        public const string createShoppingCart     = "/api/v1/shoppingcarts/create";  
        public const string addToShoppingCart      = "/api/v1/shoppingcarts/add_to_shopping_cart";
        public const string removeFromShoppingCart = "/api/v1/shoppingcarts/remove_from_shopping_cart";
        public const string resetShoppingCart      = "/api/v1/shoppingcarts/reset_shopping_cart";
        public const string buyShoppingCart        = "/api/v1/shoppingcarts/buy_shopping_cart";

    }
}