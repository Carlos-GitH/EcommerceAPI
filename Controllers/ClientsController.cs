using EcommerceAPI.DTOs;
using EcommerceAPI.Filters;
using EcommerceAPI.Models;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/clients")]
    [LogActionFilter()]
    [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
    public class ClientsController : ControllerBase
    {
        private readonly ClientsRepository _clientRepository;
        private readonly ClientsService _clientService;
        private readonly TokenService _tokenService;
        public ClientsController(ClientsRepository clientRepository, ClientsService clientService, TokenService tokenService)
        {
            _clientRepository = clientRepository;
            _clientService = clientService;
            _tokenService = tokenService;
        }
        [HttpGet("")]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetAll()
        {
            // var clients = await _dbContext.Clients.ToListAsync();
            var clients = await _clientRepository.GetAll();

            if(clients is null) return NotFound("Clients not found");

            

            var getClientsDTO = new GetClientsDTO
            {
                clients = clients.Select(c => new GetClientDTO
                {
                    id          = c.id,
                    name        = c.name,
                    cpf_cnpj    = c.cpf_cnpj,
                    phone       = c.phone is null ? "" : c.phone,
                    email       = c.email,
                    street      = c.street,
                    number      = c.number,
                    complement  = c.complement is null ? "" : c.complement,
                    city        = c.city,
                    state       = c.state,
                    postal_code = c.postal_code,
                    country     = c.country
                }).ToList()
            };

            getClientsDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientsDTO);
        }

        [HttpGet("id/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetClient(int id)
        {
            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, id)) return Unauthorized("Unauthorized");
            }
            var client   = await _clientRepository.GetById(id);
            if (client is null) return NotFound("Client not found");
            var getClientDTO = new GetClientWithTokenDTO
            {
                id          = client.id,
                name        = client.name,
                cpf_cnpj    = client.cpf_cnpj,
                phone       = client.phone is null ? "" : client.phone,
                email       = client.email,
                street      = client.street,
                number      = client.number,
                complement  = client.complement is null ? "" : client.complement,
                city        = client.city,
                state       = client.state,
                postal_code = client.postal_code,
                country     = client.country
            };
            getClientDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientDTO);
        }

        // [HttpGet("password/{id}")]
        // public async Task<IActionResult> GetClientPassword(int id)
        // {
        //     // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
        //     var client = await _clientRepository.GetById(id);

        //     if (client is null) return NotFound("Client not found");

        //     var getClientDTO = new GetClientWithPasswordDTO
        //     {
        //         id          = client.id,
        //         name        = client.name,
        //         cpf_cnpj    = client.cpf_cnpj,
        //         phone       = client.phone is null ? "" : client.phone,
        //         email       = client.email,
        //         password    = client.password,
        //         street      = client.street,
        //         number      = client.number,
        //         complement  = client.complement is null ? "" : client.complement,
        //         city        = client.city,
        //         state       = client.state,
        //         postal_code = client.postal_code,
        //         country     = client.country,
        //         // token       = await _tokenService.Create()
        //     };
        //     return Ok(getClientDTO);
        // }

        [HttpGet("personal_info/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetClientPersonalInfo(int id)
        {
            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, id)) return Unauthorized("Unauthorized");
            }
            var client = await _clientRepository.GetById(id);

            if (client is null) return NotFound("Client not found");

            var getClientPersonalInfoDTO = new GetClientPersonalInfoDTO
            {
                name     = client.name,
                cpf_cnpj = client.cpf_cnpj,
                phone    = client.phone is null ? "" : client.phone,
                email    = client.email
            };
            getClientPersonalInfoDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientPersonalInfoDTO);
        }

        [HttpPost("get_by_cpf_cnpj")]
        [Consumes("application/json")]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetClientByCpfCnpj([FromBody] GetClientByCpfCnpjDTO getCpfCnpjDTO)
        {
            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.cpf_cnpj == getCpfCnpjDTO.cpf_cnpj);
            var client = await _clientRepository.GetByCpfCnpj(getCpfCnpjDTO.cpf_cnpj);
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, client.id)) return Unauthorized("Unauthorized");
            }

            if (client is null) return NotFound("Client not found");

            var getClientDTO = new GetClientWithTokenDTO
            {
                id          = client.id,
                name        = client.name,
                cpf_cnpj    = client.cpf_cnpj,
                phone       = client.phone is null ? "" : client.phone,
                email       = client.email,
                street      = client.street,
                number      = client.number,
                complement  = client.complement is null ? "" : client.complement,
                city        = client.city,
                state       = client.state,
                postal_code = client.postal_code,
                country     = client.country
            };
            getClientDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientDTO);
        }

        [HttpPost("get_by_email")]
        [Consumes("application/json")]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetClientByEmail([FromBody] GetClientByEmailDTO getClientByEmailDTO)
        {
            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.email == getClientByEmailDTO.email);
            var client = await _clientRepository.GetByEmail(getClientByEmailDTO.email);
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, client.id)) return Unauthorized("Unauthorized");
            }


            if (client is null) return NotFound("Client not found");

            var getClientDTO = new GetClientWithTokenDTO
            {
                id          = client.id,
                name        = client.name,
                cpf_cnpj    = client.cpf_cnpj,
                phone       = client.phone is null ? "" : client.phone,
                email       = client.email,
                street      = client.street,
                number      = client.number,
                complement  = client.complement is null ? "" : client.complement,
                city        = client.city,
                state       = client.state,
                postal_code = client.postal_code,
                country     = client.country
            };
            getClientDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientDTO);
        }

        [HttpGet("address/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> GetClientAddressInfo(int id)
        {
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, id)) return Unauthorized("Unauthorized");
            }

            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
            var client = await _clientRepository.GetById(id);

            if (client is null) return NotFound("Client not found");

            var getClientAddressInfoDTO = new GetClientAddressInfoDTO
            {
                street      = client.street,
                number      = client.number,
                complement  = client.complement is null ? "" : client.complement,
                city        = client.city,
                state       = client.state,
                postal_code = client.postal_code,
                country     = client.country
            };
            getClientAddressInfoDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(getClientAddressInfoDTO);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> Create([FromBody] CreateClientDTO createClientDTO)
        {
            var client = new Client
            {
                name        = createClientDTO.name,
                cpf_cnpj    = createClientDTO.cpf_cnpj,
                phone       = createClientDTO.phone is null ? "" : createClientDTO.phone,
                email       = createClientDTO.email,
                password    = createClientDTO.password,
                street      = createClientDTO.street,
                number      = createClientDTO.number,
                complement  = createClientDTO.complement is null ? "" : createClientDTO.complement,
                city        = createClientDTO.city,
                state       = createClientDTO.state,
                postal_code = createClientDTO.postal_code,
                country     = createClientDTO.country
            };
            await _clientRepository.Create(client);
            var createdClient = await _clientRepository.GetByCpfCnpj(client.cpf_cnpj);
            await _clientService.CreateShoppingCart(createdClient.id);
            var clientWithToken = new GetClientWithTokenDTO
            {
                id          = createdClient.id,
                name        = createdClient.name,
                cpf_cnpj    = createdClient.cpf_cnpj,
                phone       = createdClient.phone is null ? "" : createdClient.phone,
                email       = createdClient.email,
                street      = createdClient.street,
                number      = createdClient.number,
                complement  = createdClient.complement is null ? "" : createdClient.complement,
                city        = createdClient.city,
                state       = createdClient.state,
                postal_code = createdClient.postal_code,
                country     = createdClient.country
            };
            clientWithToken.token = await _tokenService.GenerateAndSaveToken();
            return Ok(clientWithToken);
        }

        [HttpPost("register")]
        [Consumes("application/json")]
        public async Task<IActionResult> Register([FromBody] CreateClientDTO createClientDTO)
        {
            var client = new Client
            {
                name        = createClientDTO.name,
                cpf_cnpj    = createClientDTO.cpf_cnpj,
                phone       = createClientDTO.phone is null ? "" : createClientDTO.phone,
                email       = createClientDTO.email,
                password    = createClientDTO.password,
                street      = createClientDTO.street,
                number      = createClientDTO.number,
                complement  = createClientDTO.complement is null ? "" : createClientDTO.complement,
                city        = createClientDTO.city,
                state       = createClientDTO.state,
                postal_code = createClientDTO.postal_code,
                country     = createClientDTO.country
            };
            await _clientRepository.Create(client);
            var createdClient = await _clientRepository.GetByCpfCnpj(client.cpf_cnpj);
            await _clientService.CreateShoppingCart(createdClient.id);
            return Ok("Register created successfully");
        }

        [HttpPost("login")]
        [Consumes("application/json")]
        public async Task<IActionResult> Login([FromBody] LoginClientDTO loginClientDTO)
        {
            var client = await _clientRepository.GetByEmail(loginClientDTO.email);
            if (client is null) return NotFound("Client not found");
            if (client.password != loginClientDTO.password) return BadRequest("Invalid password");
            var generatedToken = await _tokenService.GenerateAndSaveToken();
            var logguedClient = new LogguedClientDTO
            {
                token = generatedToken,
                id    = client.id,
                email = client.email
            };
            return Ok(logguedClient);
        }

        [HttpPut("edit/{id}")]
        [Consumes("application/json")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDTO updateClientDTO)
        {
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, id)) return Unauthorized("Unauthorized");
            }
            // var client = await _dbContext.Clients.FirstOrDefaultAsync(c => c.id == id);
            var client = await _clientRepository.GetById(id);
            if (client is null) return NotFound("Client not found");

            client.name        = updateClientDTO.name is null ? client.name : updateClientDTO.name;
            client.cpf_cnpj    = updateClientDTO.cpf_cnpj is null ? client.cpf_cnpj : updateClientDTO.cpf_cnpj;
            client.phone       = updateClientDTO.phone is null ? client.phone : updateClientDTO.phone;
            client.email       = updateClientDTO.email is null ? client.email : updateClientDTO.email;
            client.street      = updateClientDTO.street is null ? client.street : updateClientDTO.street;
            client.number      = updateClientDTO.number is null ? client.number : updateClientDTO.number;
            client.complement  = updateClientDTO.complement is null ? client.complement : updateClientDTO.complement;
            client.city        = updateClientDTO.city is null ? client.city : updateClientDTO.city;
            client.state       = updateClientDTO.state is null ? client.state : updateClientDTO.state;
            client.postal_code = updateClientDTO.postal_code is null ? client.postal_code : updateClientDTO.postal_code;
            client.country     = updateClientDTO.country is null ? client.country : updateClientDTO.country;

            await _clientRepository.Update(client);
            var updatedClient = new updatedClientDTO
            {
                id          = client.id,
                cpf_cnpj    = client.cpf_cnpj,
                phone       = client.phone is null ? "" : client.phone,
                email       = client.email,
                street      = client.street,
                number      = client.number,
                complement  = client.complement is null ? "" : client.complement,
                city        = client.city,
                state       = client.state,
                postal_code = client.postal_code,
                country     = client.country
            };
            updatedClient.token = await _tokenService.GenerateAndSaveToken();
            return Ok(updatedClient);
        }

        [HttpDelete("delete/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        [TypeFilter(typeof(SellerAuthorizeActionFilter))]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, id)) return Unauthorized("Unauthorized");
            }
            
            var client = await _clientRepository.GetById(id);
            if (client is null) return NotFound("Client not found");
            await _clientRepository.Delete(client);
            var token = await _tokenService.GenerateAndSaveToken();
            return Ok(token);
        }
    }
}