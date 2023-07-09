using DropcopyGenerator;

namespace OMS.OrderSenders
{
    internal class InnerOrderSender : IOrderSender
    {
        private readonly OrderProcess _orderProcess;
        private readonly OrderGenerator _orderGenerator;

        public InnerOrderSender(OrderProcess orderProcess, OrderGenerator orderGenerator)
        {
            _orderProcess = orderProcess;
            _orderGenerator = orderGenerator;
        }

        public void Start()
        {
            while (true)
            {
                var firstExecution = _orderGenerator.NewOrder();
                _orderProcess.Process(firstExecution);

                ExecutionReport lastEr = null!;
                while ((lastEr = _orderGenerator.NextER(lastEr ?? firstExecution)) != null)
                {
                    _orderProcess.Process(lastEr);
                }

                //foreach (var _ in Enumerable.Range(0, (int)firstExecution.Quantity))
                //{
                //    lastEr = _orderGenerator.NextER(lastEr ?? firstExecution);
                //    if (lastEr != null)
                //        _orderProcess.Process(lastEr);
                //}
            }
        }

        public void Stop()
        {
        }
    }
}
