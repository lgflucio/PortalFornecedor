using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServicoWindows
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMyHostedService _myHostedService;

        public Worker(ILogger<Worker> logger, IMyHostedService myHostedService)
        {
            _logger = logger;
            _myHostedService = myHostedService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _myHostedService.DoWork();
                await Task.Delay(1000, stoppingToken);
                _logger.LogInformation("Worker Finishing at: {time}", DateTimeOffset.Now);
            }
        }
    }
}
