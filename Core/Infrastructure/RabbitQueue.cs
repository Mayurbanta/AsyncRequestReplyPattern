using Core.Infrastructure.Rabbit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure;
public class RabbitQueue<T> : IBaseQueue<T>
    where T : class,INotification
{
    private readonly ILogger<RabbitQueue<T>> _logger;
    private readonly IRabbitProducer _rabbitProducer;
    private readonly IRabbitConsumer<T> _rabbitConsumer;

    public RabbitQueue(ILogger<RabbitQueue<T>> logger,
        IRabbitProducer rabbitProducer, IRabbitConsumer<T> rabbitConsumer)
    {
        _logger = logger;
        _rabbitProducer = rabbitProducer;
        _rabbitConsumer = rabbitConsumer;
    }
    public async Task EnqueueAsync(INotification notifyData, CancellationToken cancelToken = default)
    {
        if (cancelToken.IsCancellationRequested)
            throw new OperationCanceledException();

        await _rabbitProducer.SendMessage(notifyData);
        _logger.LogInformation("Rabbit message sent !");

    }

    public async Task<INotification?> DequeueAsync(CancellationToken cancelToken)
    {
        if (cancelToken.IsCancellationRequested)
            throw new OperationCanceledException();

        _logger.LogInformation("Trying to dequeue if any message exists !");
        var data= await _rabbitConsumer.ReadMessage();
        if (data != null)
        {
            return data;
        }
        else return null;
    }
}
