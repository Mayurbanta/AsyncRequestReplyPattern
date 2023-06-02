using MediatR;

namespace Core.Infrastructure.Rabbit;

public interface IRabbitConsumer<T> where T : class, INotification
{
    Task<T?> ReadMessage();
}