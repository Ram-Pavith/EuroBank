using EuroBankAPI.Data;
using EuroBankAPI.Repository;
using EuroBankAPI.Repository.IRepository;
using System.Net.Http;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;

namespace ServiceChargesService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IUnitOfWork _unitOfWork;
        private readonly IServiceProvider _serviceProvider;
        private EuroBankContext _euroBankContext;
        private readonly double _minBalance = 15000;
        public Worker(ILogger<Worker> logger,EuroBankContext euroBankContext,IServiceProvider serviceProvider)
        {
            _logger = logger;
            _euroBankContext = euroBankContext;
            _serviceProvider = serviceProvider;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _euroBankContext.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<EuroBankContext>();
                    int totalRows = await dbContext.Accounts.CountAsync();
                    _logger.LogInformation("Total rows in Accounts is " + totalRows);
                }
                /*var accounts = _euroBankContext.Accounts.Where(x => x.Balance < _minBalance).ToList();
                foreach (var account in accounts)
                {
                    _logger.LogInformation("Account " + account.AccountId + " has Balance of " + account.Balance);

                }*/
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}