using PM.Collections;

namespace OMS.Repositories
{
    public class PmOrderRepository : IOrderRepository
    {
        private readonly PmList<Order> _orders;
        private readonly Dictionary<string, Order> _ordersByClOrderId = new();

        public PmOrderRepository(string filePath)
        {
            _orders = new PmList<Order>(filePath);
        }

        public void AddOrder(Order order)
        {
            _ordersByClOrderId[order.ClOrdId] = _orders.AddPersistent(order);
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _orders;
        }

        public Order GetOrderByClOrdId(string clOrdID)
        {
            if (_ordersByClOrderId.TryGetValue(clOrdID, out var result))
            {
                return result;
            }

            foreach (var order in _orders)
            {
                if (order.ClOrdId == clOrdID) return order;
            }
            return null;
        }

        public void UpdateOrder(Order order)
        {
            var pmOrder = GetOrderByClOrdId(order.ClOrdId);
            if (pmOrder != null)
            {
                pmOrder.Status = order.Status;
                pmOrder.ExecutedQuantity = order.ExecutedQuantity;
            }
        }
    }
}
