using Core.Contracts;
using Core.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.BackgroundServices;

public sealed class QueuedHostedService<T>
    : BackgroundService
    where T : class, INotification
{
    private readonly IBaseQueue<T> _queue;
    private readonly ILogger<QueuedHostedService<T>> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public QueuedHostedService(ILogger<QueuedHostedService<T>> logger,
        IServiceScopeFactory scopeFactory, IBaseQueue<T> queue)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _queue = queue;
    }

    protected override  Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(QueuedHostedService<T>)} is running.");
        return ProcessTaskQueueAsync(stoppingToken);
    }

    private async Task ProcessTaskQueueAsync(CancellationToken cancelToken)
    {
        await Task.Delay(TimeSpan.FromSeconds(15));

        while (!cancelToken.IsCancellationRequested)
        {
            try
            {

                _logger.LogInformation("Waiting for new queue message.");
                var backgroundTask = await _queue.DequeueAsync(cancelToken);

                if (backgroundTask != null)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();

                    _logger.LogInformation("Running task {TaskType}", backgroundTask.GetType());
                    await publisher.Publish(backgroundTask, cancelToken);
                    _logger.LogInformation("Completed task {TaskType}", backgroundTask.GetType());
                }
                await Task.Delay(TimeSpan.FromSeconds(5));

            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if stoppingToken was signaled
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred executing task work item.");
            }
        }
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(QueuedHostedService<T>)} is stopping.");
        await base.StopAsync(stoppingToken);
    }
}
