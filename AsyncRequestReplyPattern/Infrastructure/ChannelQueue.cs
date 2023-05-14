using MediatR;
using System.Threading.Channels;

namespace AsyncRequestReplyPattern.Infrastructure;

public class ChannelQueue<T> : IChannelQueue<T> where T : class, new()
{
    private readonly Channel<INotification> _queue = Channel.CreateUnbounded<INotification>();

    public async Task EnqueueAsync(INotification task, CancellationToken cancelToken = default)
    {
        await _queue.Writer.WriteAsync(task, cancelToken);
    }

    public async Task<INotification> DequeueAsync(CancellationToken cancelToken)
    {
        return await _queue.Reader.ReadAsync(cancelToken);
    }

    
}
