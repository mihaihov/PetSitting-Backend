
using Microsoft.Extensions.Hosting;
using PetSitting.Application.Interfaces.Services;

namespace PetSitting.Infrastructure.Services
{
    public class StripeWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        public StripeWorker(IBackgroundTaskQueue backgroundTaskQueue)
        {
           _backgroundTaskQueue = backgroundTaskQueue ?? 
                throw new ArgumentNullException(nameof(backgroundTaskQueue)); 
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _backgroundTaskQueue.DequeueAsync(stoppingToken);
                if (workItem is not null)
                {
                    await workItem(stoppingToken);
                }
            }
        }
    }
}