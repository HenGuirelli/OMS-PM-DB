using Microsoft.AspNetCore.Mvc;
using OMS.Repositories;

namespace OMS.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        [HttpGet("all")]
        public IEnumerable<Order> Get()
        {
            return _orderRepository.GetAllOrders();
        }

        [HttpGet("{clOrdId}")]
        public Order Get(string clOrdId)
        {
            return _orderRepository.GetOrderByClOrdId(clOrdId);
        }
    }
}