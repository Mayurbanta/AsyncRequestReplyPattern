using Core.Mediatr.Notifications;
using MediatR;
using OrderWorker.Services;

namespace OrderWorker.Mediatr.Handlers;

public class OrderJobHandler
    : INotificationHandler<LongRunningJobNotification<OrderService>>
{
    public OrderJobHandler()
    {

    }

    public Task Handle(LongRunningJobNotification<OrderService> notification, CancellationToken cancellationToken)
    {
        var service = new OrderService();
        service.Handle(notification.JobId);

        return Task.CompletedTask;
    }
}
