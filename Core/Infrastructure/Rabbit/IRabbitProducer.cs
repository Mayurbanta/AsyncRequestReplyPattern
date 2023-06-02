namespace Core.Infrastructure.Rabbit;

public interface IRabbitProducer
{
    Task SendMessage<T>(T message);
}