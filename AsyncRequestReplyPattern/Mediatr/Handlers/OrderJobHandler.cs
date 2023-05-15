using AsyncRequestReplyPattern.Services;
using Core.Mediatr.Notifications;
using MediatR;

namespace AsyncRequestReplyPattern.Mediatr.Handlers;

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
