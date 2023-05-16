using Core.Infrastructure;
using Core.Mediatr.Notifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaxWorker.Services;

namespace TaxWorker.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TaxController : ControllerBase
{
    private readonly ILogger<TaxController> _logger;
    private readonly IChannelQueue<TaxService> _channelQueue;
    public TaxController(ILogger<TaxController> logger, IChannelQueue<TaxService> channelQueue)
	{
        _logger = logger;
        _channelQueue = channelQueue;
    }

    [HttpPost("/api/taxrequest")]
    public async Task<IActionResult> ProcessTax()
    {
        var jobId = Guid.NewGuid();
        await _channelQueue.EnqueueAsync(new LongRunningJobNotification<TaxService>(jobId));
        return Ok(jobId);
    }
}
