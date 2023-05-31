using PM.Collections;

namespace OMS.Repositories
{
    public class PmOrderRepository : IOrderRepository
    {
        private readonly PmList<Order> _orders;

        public PmOrderRepository(string filePath)
        {
            _orders = new PmList<Order>(filePath);
        }

        public void AddOrder(Order order)
        {
            _orders.AddPersistent(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders;
        }

        public Order GetOrderByClOrdId(string clOrdID)
        {
            foreach (var order in _orders)
            {
                if (order.ClOrdId == clOrdID) return order;
            }
            return null;
        }

        public void UpdateOrder(Order order)
        {
            // Nothing to see here
        }
    }
}
