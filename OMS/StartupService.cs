using OMS.OrderSenders;
using OMS.Repositories;

namespace OMS
{
    public class StartupService : IHostedService
    {
        private readonly IOrderSender _orderSender;

        public StartupService(IOrderSender orderSender)
        {
            _orderSender = orderSender;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _orderSender.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _orderSender.Stop();
            return Task.CompletedTask;
        }
    }
}
