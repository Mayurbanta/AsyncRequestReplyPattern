using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Rabbit;
public class RabbitProducer : IRabbitProducer
{
    public async Task SendMessage<T>(T message)
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost"
        };

        var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare("myqueue", exclusive: false);

        var json = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(json);

        channel.BasicPublish(exchange: "", routingKey: "myqueue", body: body);

        await Task.CompletedTask;
    }

}
