namespace AsyncRequestReplyPattern.Services;

public interface IOrderService : IJobService
{

}

public class OrderService : IOrderService
{
    public void Handle(Guid jobId)
    {
        Console.WriteLine("came to handle method !!!");
    }
}

