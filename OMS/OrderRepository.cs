namespace OMS
{
    public class OrderRepository
    {
        private static List<Order> _orders = new();

        public static IEnumerable<Order> Orders => _orders;

        public static void Add(Order order)
        {
            _orders.Add(order);
        }

        public static Order? GetOrderByClOrdID(string clOrderID)
        {
            return _orders.SingleOrDefault(x => x.ClOrdId == clOrderID);
        }

        public static void AddExecutionReport(ExecutionReport executionReport)
        {
            var order = GetOrderByClOrdID(executionReport.ClOrdId);
            if (order.OrderId is null)
            {
                order.OrderId = executionReport.OrderId;
                order.ExecutionReports = new List<ExecutionReport>();
            }
            order.ExecutionReports.Add(executionReport);
        }

        public static void CreateOrder(ExecutionReport er)
        {
            _orders.Add(new Order
            {
                Account = er.Account,
                ClOrdId = er.ClOrdId,
                ExecutedQuantity = er.CumQty,
                OrderId= er.OrderId,
                Price = er.Price,
                Quantity = er.Quantity,
                Status = er.Status,
                Symbol = er.Symbol
            });
        }
    }
}
