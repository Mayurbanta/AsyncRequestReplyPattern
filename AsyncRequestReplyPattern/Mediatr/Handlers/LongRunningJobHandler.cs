using AsyncRequestReplyPattern.Mediatr.Notifications;
using AsyncRequestReplyPattern.Services;
using MediatR;

namespace AsyncRequestReplyPattern.Mediatr.Handlers;

public class LongRunningJobHandler
    : INotificationHandler<LongRunningJobNotification<OrderService>>
{
    public LongRunningJobHandler()
    {

    }

    public Task Handle(LongRunningJobNotification<OrderService> notification, CancellationToken cancellationToken)
    {
        var service = new OrderService();
        service.Handle(notification.JobId);

        
        return Task.CompletedTask;
    }
}
