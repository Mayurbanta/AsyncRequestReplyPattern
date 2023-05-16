using Core.Mediatr.Notifications;
using MediatR;
using TaxWorker.Services;

namespace TaxWorker.Mediatr.Handlers;

public class TaxJobHandler
    : INotificationHandler<LongRunningJobNotification<TaxService>>
{
    public Task Handle(LongRunningJobNotification<TaxService> notification, CancellationToken cancellationToken)
    {
        var service = new TaxService();
        service.Handle(notification.JobId);

        return Task.CompletedTask;
    }
}
