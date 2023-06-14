using OMS.Repositories.Accounts;
using PM.Collections;
using PM.Transactions;

namespace OMS.Repositories
{
    public class PmTransactionOrderRepository : IOrderRepository
    {
        private readonly PmList<Order> _orders;
        private readonly Dictionary<string, Order> _ordersByClOrderId = new();
        private readonly PmAccountsRepository _accountsRepository = new();

        public PmTransactionOrderRepository(string filePath)
        {
            _orders = new PmList<Order>(filePath);
        }

        public void AddOrder(Order order)
        {
            var account = _accountsRepository.GetAccount(order.Account);
            try
            {
                account.Transaction(() =>
                {
                    _ordersByClOrderId[order.ClOrdId] = account.Orders.AddPersistent(order);
                    account.ContaCorrente.Value -= order.Price;
                });
            }
            catch { }
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
            pmOrder.Status = order.Status;
            pmOrder.ExecutedQuantity = order.ExecutedQuantity;
        }
    }
}
