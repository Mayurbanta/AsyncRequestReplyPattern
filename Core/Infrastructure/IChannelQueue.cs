﻿using MediatR;

namespace Core.Infrastructure;

public interface IChannelQueue<T> where T : class, new()
{
    Task EnqueueAsync(INotification task, CancellationToken cancelToken = default);
    Task<INotification> DequeueAsync(CancellationToken cancelToken);
}
