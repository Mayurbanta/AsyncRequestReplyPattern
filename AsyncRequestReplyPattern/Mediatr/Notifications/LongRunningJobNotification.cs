using MediatR;

namespace AsyncRequestReplyPattern.Mediatr.Notifications;

public record LongRunningJobNotification<T>(Guid JobId) : INotification;
