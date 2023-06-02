using Core.Contracts;
using MediatR;

namespace Core.Mediatr.Notifications;

public record LongRunningJobNotification<T>(Guid JobId) : INotification
    where T: IJobService;
