namespace OMS
{
    public class StartupService : IHostedService
    {
        private IServiceProvider _services;
        private Acceptor _acceptor = new();

        public StartupService(IServiceProvider services)
        {
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _acceptor.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _acceptor.Stop();
            return Task.CompletedTask;
        }
    }
}
