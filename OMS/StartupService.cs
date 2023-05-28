namespace OMS
{
    public class StartupService : IHostedService
    {
        private IServiceProvider _services;
        private readonly Initiator _orderSender = new();

        public StartupService(IServiceProvider services)
        {
            _services = services;
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
