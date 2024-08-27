using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommerceAPI.Models;

namespace EcommerceAPI.DTOs
{
    public class CreateClientDTO
    {
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
    }

    public class GetClientsDTO
    {
        public List<GetClientDTO> clients { get; set; }
        public string token { get; set; }
    }
    
    public class GetClientDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string? complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
    }

    public class GetClientWithTokenDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string? complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string token { get; set; }
    }

    public class GetClientPersonalInfoDTO
    {
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string token { get; set; }
    }

    public class GetClientAddressInfoDTO
    {
        public string street { get; set; }
        public string number { get; set; }
        public string? complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string token { get; set; }
    }

    public class GetClientByEmailDTO
    {
        public string email { get; set; }
    }

    public class GetClientByCpfCnpjDTO
    {
        public string cpf_cnpj { get; set; }
    }

    public class UpdateClientDTO
    {
        public string? name { get; set; }
        public string? cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string? email { get; set; }
        public string? street { get; set; }
        public string? number { get; set; }
        public string? complement { get; set; }
        public string? city { get; set; }
        public string? state { get; set; }
        public string? postal_code { get; set; }
        public string? country { get; set; }
    }

    public class updatedClientDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
        public string token { get; set; }
    }

    public class LoginClientDTO
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    
    public class LogguedClientDTO
    {
        public string token { get; set; }
        public int id { get; set; }
        public string email { get; set; }
    }

    public class GetClientWithPasswordDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string cpf_cnpj { get; set; }
        public string? phone { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string street { get; set; }
        public string number { get; set; }
        public string? complement { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string postal_code { get; set; }
        public string country { get; set; }
    }
}