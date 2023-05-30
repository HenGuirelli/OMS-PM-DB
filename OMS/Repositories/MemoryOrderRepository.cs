using System.Collections.Concurrent;

namespace OMS.Repositories
{
    public class MemoryOrderRepository : IOrderRepository
    {
        private readonly ConcurrentDictionary<string, Order> _orders = new();

        public void AddOrder(Order order)
        {
            _orders[order.ClOrdId] = order;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders.Values.ToList();
        }

        public Order GetOrderByClOrdId(string clOrdID)
        {
            _orders.TryGetValue(clOrdID, out var order);
            return order;
        }

        public void UpdateOrder(Order order)
        {
            _orders[order.ClOrdId] = order;
        }
    }
}
