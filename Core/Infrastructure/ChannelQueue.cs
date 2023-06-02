using MediatR;
using System.Threading.Channels;

namespace Core.Infrastructure;

public class ChannelQueue<T> : IBaseQueue<T> 
    where T : class,INotification, new()
{
    private readonly Channel<INotification> _queue = Channel.CreateUnbounded<INotification>();

    public async Task EnqueueAsync(INotification notifyData, CancellationToken cancelToken = default)
    {
        await _queue.Writer.WriteAsync(notifyData, cancelToken);
    }

    public async Task<INotification> DequeueAsync(CancellationToken cancelToken)
    {
        return await _queue.Reader.ReadAsync(cancelToken);
    }


}
