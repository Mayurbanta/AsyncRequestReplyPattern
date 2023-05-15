using MediatR;

namespace Core.Mediatr.Notifications;

public record LongRunningJobNotification<T>(Guid JobId) : INotification;
