using Microsoft.Extensions.Hosting;

namespace Common.UnitTest
{
    public class SecurityMicroserviceHostedService : IHostedService
    {
        private Task _runningTask;
        private CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = new CancellationTokenSource();

            _runningTask = Task.Run(() =>
            {
                string[] args = Array.Empty<string>();
                Program.Main(args); 
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
    }

}
