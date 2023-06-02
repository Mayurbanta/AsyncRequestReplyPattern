using MediatR;

namespace Core.Infrastructure;

public interface IBaseQueue<T> where T : class,INotification
{
    Task EnqueueAsync(INotification notifyData, CancellationToken cancelToken = default);
    Task<INotification?> DequeueAsync(CancellationToken cancelToken);
}
