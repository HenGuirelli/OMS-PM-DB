namespace OMS.Repositories
{
    public interface IOrderRepository
    {
        void AddOrder(Order order);
        void UpdateOrder(Order order);
        IEnumerable<Order> GetAllOrders();
        Order GetOrderByClOrdId(string clOrdID);
    }
}
