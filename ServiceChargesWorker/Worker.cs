using EuroBankAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace ServiceChargesWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private AppDbContext _context;

        public Worker(ILogger<Worker> logger, AppDbContext euroBankContext, IServiceScopeFactory serviceProvider)
        {
            _logger = logger;
            _context = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>();
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _context.Dispose();
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int totalRows = await _context.Accounts.CountAsync();
                _logger.LogInformation("Total rows in Accounts is " + totalRows);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}