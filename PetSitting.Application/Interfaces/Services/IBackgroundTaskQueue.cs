using System;
using System.Threading.Tasks;

namespace PetSitting.Application.Interfaces.Services
{
    public interface IBackgroundTaskQueue
    {
        void Enqueue(Func<CancellationToken, Task> workItem);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}