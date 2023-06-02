using Core.Infrastructure;
using Core.Mediatr.Notifications;
using Microsoft.AspNetCore.Mvc;
using OrderWorker.Services;

namespace OrderWorker.Controllers;
[Route("api/[controller]")]
[ApiController]
public class OrderController
    : ControllerBase
{
    private readonly ILogger<OrderController> _logger;
    private readonly IBaseQueue<LongRunningJobNotification<IOrderService>> _baseQueue;

    public OrderController(ILogger<OrderController> logger,
        IBaseQueue<LongRunningJobNotification<IOrderService>> baseQueue)
    {
        _logger = logger;
        _baseQueue = baseQueue;
    }

    [HttpPost("/api/orderrequest")]
    public async Task<IActionResult> ProcessOrder()
    {
        var jobId = Guid.NewGuid();
        await _baseQueue.EnqueueAsync(new LongRunningJobNotification<IOrderService>(jobId));
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
