using OMS.Repositories;

namespace OMS
{
    public class StartupService : IHostedService
    {
        private readonly IServiceProvider _services;
        private readonly Initiator _orderSender;

        public StartupService(IServiceProvider services)
        {
            _services = services;
            _orderSender = new (_services.GetRequiredService<IOrderRepository>());
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
