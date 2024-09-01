using EcommerceAPI.DTOs;
using EcommerceAPI.Models;
using Microsoft.AspNetCore.Mvc;
using EcommerceAPI.Repositories;
using EcommerceAPI.Services;
using EcommerceAPI.Constants;
using EcommerceAPI.Filters;

namespace EcommerceAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/orders")]
    [LogActionFilter()]
    // [TypeFilter(typeof(AsyncAuthorizeActionFilter))]
    // [TypeFilter(typeof(SellerAuthorizeActionFilter))]
    public class OrdersControllers : ControllerBase
    {
        private readonly OrdersRepository _orderRepository;
        private readonly OrdersService _orderService;
        private readonly TokenService _tokenService;
        private readonly ClientsService _clientService;
        public OrdersControllers(OrdersRepository orderRepository, OrdersService orderService, TokenService tokenService, ClientsService clientService)
        {
            _orderRepository = orderRepository;
            _orderService    = orderService;
            _tokenService    = tokenService;
            _clientService   = clientService;
        }

        [HttpGet("")]
        public async Task<ActionResult <IEnumerable<Order>>> GetAll()
        {
            var orders          = await _orderRepository.GetAll();
            var ordersWithToken = new GetOrdersDetailedWithTokenDTO
            {
                orders = orders
            };
            ordersWithToken.token = await _tokenService.GenerateAndSaveToken();
            return Ok(ordersWithToken);
        }
        [HttpGet("id/{id}")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        public async Task<ActionResult<IEnumerable<OrderItemDTO>>> GetOrder(int id)
        {
            var order     = await _orderRepository.GetById(id);
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, order.client_id)) return Unauthorized("Unauthorized");
            }
            if (order is null) return NotFound();
            var orderWithToken = new GetOrderWithTokenDTO
            {
                id            = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            orderWithToken.token = await _tokenService.GenerateAndSaveToken();
            return Ok(orderWithToken);
        }

        [HttpPost("client/id")]
        [Consumes("application/json")]
        [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        public async Task<ActionResult> GetByClientID([FromBody] GetByClientIDDTO client_id)
        {
            var reference = Request.Headers["reference"];
            if (string.IsNullOrEmpty(reference))
            {
                var headerId = Request.Headers["id"];
                if (!_clientService.ValidateProprietaryClient(headerId, client_id.client_id)) return Unauthorized("Unauthorized");
            }
            var orders          = await _orderRepository.GetByClientId(client_id.client_id);
            var ordersWithToken = new GetOrdersDetailedWithTokenDTO
            {
                orders = orders
            };
            ordersWithToken.token = await _tokenService.GenerateAndSaveToken();
            return Ok(ordersWithToken);
        }

        [HttpPost("cpf_cnpj")]
        [Consumes("application/json")]
        public async Task<ActionResult> GetByCpfCnpj([FromBody] GetByCpfCnpjDTO getCpfCnpjDTO)
        {
            var orders           = await _orderRepository.GetByCpfCnpj(getCpfCnpjDTO.cpf_cnpj);
             var ordersWithToken = new GetOrdersDetailedWithTokenDTO
            {
                orders = orders
            };

            ordersWithToken.token = await _tokenService.GenerateAndSaveToken();
            return Ok(ordersWithToken);
        }

        [HttpPost("create")]
        [Consumes("application/json")]
        // [TypeFilter(typeof(ClientAuthorizeActionFilter))]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderDTO createOrderDTO, HttpClient httpClient)
        {
            string apiKey    = Request.Headers["api_key"];
            var paymentToken = await _orderService.AutenticatePayment(httpClient, apiKey);
            if (paymentToken == null) return Unauthorized("Unauthorized, token missing or invalid");
            
            // var reference = Request.Headers["reference"];
            // if (string.IsNullOrEmpty(reference))
            // {
            //     var headerId = Request.Headers["id"];
            //     if (!_clientService.ValidateProprietaryClient(headerId, createOrderDTO.client_id)) return Unauthorized("Unauthorized");
            // }
            decimal totalPrice = 0;
            createOrderDTO.payment_info.value = totalPrice;
            var teste = await _orderService.Pay(httpClient, createOrderDTO.payment_info, apiKey, paymentToken);

            
            foreach (var item in createOrderDTO.items)
            {
                var price = await _orderService.VerifyStock(item);
                if(price == 0) return BadRequest("Product out of stock");
                else if(price == -1) return NotFound("Product not found");
                totalPrice += _orderService.totalItemPrice(price, item.quantity);
            }
            var createdOrder = await _orderRepository.Create(createOrderDTO, totalPrice);
            foreach (var item in createOrderDTO.items)
            {
                CreateOrderItemDTO createOrderItemDTO = new CreateOrderItemDTO
                {
                    order_id   = createdOrder.id,
                    product_id = item.product_id,
                    quantity   = item.quantity
                };
                await _orderService.CreateOrderItem(createOrderItemDTO, createdOrder.id);
                await _orderService.ReduceStock(item);
            }
            var orderToReturn    = await _orderRepository.GetById(createdOrder.id);
            var orderToReturnDTO = new GetOrderWithTokenDTO
            {
                id            = orderToReturn.id,
                client_id     = orderToReturn.client_id,
                seller_id     = orderToReturn.seller_id,
                delivery_type = orderToReturn.delivery_type,
                order_status  = orderToReturn.order_status,
                total_price   = orderToReturn.total_price,
                client_name   = orderToReturn.client_name,
                seller_name   = orderToReturn.seller_name,
                order_items   = orderToReturn.order_items
            };
            
            // orderToReturnDTO.token = await _tokenService.GenerateAndSaveToken();
            return Ok(orderToReturnDTO);
        }

        [HttpPatch("confirm")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateStatusConfirmed([FromBody] OrderIdDTO orderIdDTO)
        {
            var order = await _orderRepository.GetById(orderIdDTO.id);
            if (order is null) return NotFound();
            if (order.order_status != StatusOrders.PendingConfirmation) return BadRequest("Order already confirmed");
            order.order_status = StatusOrders.Confirmed;
            await _orderRepository.UpdateOrderStatus(orderIdDTO, order.order_status);
            var orderToReturn = new GetOrderWithTokenDTO
            {
                id            = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            orderToReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(orderToReturn);
        }

        [HttpPatch("ready")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateStatusAwait([FromBody] OrderIdDTO orderIdDTO)
        {
            var order = await _orderRepository.GetById(orderIdDTO.id);
            if (order is null) return NotFound();
            if (order.order_status != StatusOrders.Confirmed) return BadRequest("Order not confirmed");
            if (order.delivery_type == "delivery")
            {
                order.order_status = StatusOrders.AwaitingDelivery;
            } else {
                order.order_status = StatusOrders.AwaitingPickup;
            }
            await _orderRepository.UpdateOrderStatus(orderIdDTO, order.order_status);
            var orderToReturn = new GetOrderWithTokenDTO
            {
                id            = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            orderToReturn.token = await _tokenService.GenerateAndSaveToken();
            
            return Ok(orderToReturn);
        }

        [HttpPatch("delivered")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateStatusDelivered([FromBody] OrderIdDTO orderIdDTO)
        {
            var order = await _orderRepository.GetById(orderIdDTO.id);
            if (order is null) return NotFound();
            if (order.order_status != StatusOrders.AwaitingDelivery || order.order_status != StatusOrders.AwaitingPickup) return BadRequest("Order not ready to be delivered");
                order.order_status = StatusOrders.Delivered;
            await _orderRepository.UpdateOrderStatus(orderIdDTO, order.order_status);
            var orderToReturn = new GetOrderWithTokenDTO
            {
                id            = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            orderToReturn.token = await _tokenService.GenerateAndSaveToken();
            
            return Ok(orderToReturn);
        }

        [HttpPatch("cancel")]
        [Consumes("application/json")]
        public async Task<ActionResult> UpdateStatusCancel([FromBody] OrderIdDTO orderIdDTO)
        {
            var order = await _orderRepository.GetById(orderIdDTO.id);
            if (order is null) return NotFound();
            if (order.order_status != StatusOrders.PendingConfirmation) return BadRequest("Order already sent or delivered. Cancel not possible");
            order.order_status = StatusOrders.Canceled;
            if (await _orderRepository.UpdateOrderStatus(orderIdDTO, order.order_status) is null) return BadRequest("Invalid Status");
            foreach(var item in order.order_items)
            {
                await _orderService.RestoreStock(item);
            }
            var orderToReturn = new GetOrderWithTokenDTO
            {
                id            = order.id,
                client_id     = order.client_id,
                seller_id     = order.seller_id,
                delivery_type = order.delivery_type,
                order_status  = order.order_status,
                total_price   = order.total_price,
                client_name   = order.client_name,
                seller_name   = order.seller_name,
                order_items   = order.order_items
            };
            orderToReturn.token = await _tokenService.GenerateAndSaveToken();
            return Ok(orderToReturn);
        }

        // public async Task<decimal> VerifyStock(OrderItemDTO item)
        // {
        //     Product product = await _orderService.GetProductById(item.product_id);
        //     if(product is null) return -1;
        //     if(product.stock < item.quantity) return 0;
        //     return product.price;
        // }

        // public async Task<Product> ReduceStock(OrderItemDTO item)
        // {
        //     Product product = await _orderService.GetProductById(item.product_id);
        //     var reducedStock = await _orderService.ReduceStock(product, item.quantity);
        //     return reducedStock;
        // }

        
    }
}