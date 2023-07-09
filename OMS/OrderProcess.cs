using OMS.Repositories;

namespace OMS
{
    internal class OrderProcess
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ExecutionReportCounter _executionReportCounter = new();

        public OrderProcess(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Process(ExecutionReport er)
        {
            _executionReportCounter.AddEr();

            var order = new Order
            {
                OrderId = er.OrderId,
                Account = er.Account,
                ClOrdId = er.ClOrdId,
                Quantity = er.Quantity,
                ExecutedQuantity = er.CumQty,
                Price = er.Price,
                Status = er.Status,
                Symbol = er.Symbol,
            };

            try
            {
                if (er.Status == 0) // NEW
                {
                    _orderRepository.AddOrder(order);
                }
                else
                {
                    _orderRepository.UpdateOrder(order);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
