using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using Newtonsoft.Json;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Rabbit;
public class RabbitConsumer<T> : IRabbitConsumer<T>
    where T : class, INotification
{
    private readonly ILogger<RabbitConsumer<T>> _logger;
    private IConnection _connection = null! ;
    private IModel _channel = null!;
    private EventingBasicConsumer _consumer = null!;
    private T? _notifyData = default;

    private const string _message = "Rabbit message received: {0}";

    public RabbitConsumer(ILogger<RabbitConsumer<T>> logger)
    {
        _logger = logger;
        ConfigureQueue();
    }

    private void ConfigureQueue()
    {
        
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare("myqueue", exclusive: false);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, eventArgs) =>
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            _logger.LogInformation(RabbitConsumer<T>._message, message);

            _notifyData = JsonConvert.DeserializeObject<T>(message);
        };
    }

    public async Task<T?> ReadMessage()
    {
        GetMessage();
        return await Task.FromResult(_notifyData);
    }

    private void GetMessage()
    {
        _channel.BasicConsume(queue: "myqueue", autoAck: true, consumer: _consumer);
    }
}
