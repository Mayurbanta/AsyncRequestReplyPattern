using AsyncRequestReplyPattern.Services;
using Core.Infrastructure;
using Core.Mediatr.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace AsyncRequestReplyPattern.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController
    : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IChannelQueue<OrderService> _channelQueue;

    public OrderController(ILogger<OrderController> logger,
        IChannelQueue<OrderService> channelQueue)
    {
        _logger = logger;
        _channelQueue = channelQueue;
    }

    [HttpPost("/api/orderrequest")]
    public async Task<IActionResult> ProcessOrder()
    {
        var jobId = Guid.NewGuid();
        await _channelQueue.EnqueueAsync(new LongRunningJobNotification<OrderService>(jobId));
        return Ok(jobId);
    }

    [HttpGet("/api/longRunning/status/{id:guid}")]
    public IActionResult GetOrderStatus(Guid id)
    {
        //var status = _statusService.GetStatus(id);
        //return Ok(status);
        throw new NotImplementedException();
    }
}
