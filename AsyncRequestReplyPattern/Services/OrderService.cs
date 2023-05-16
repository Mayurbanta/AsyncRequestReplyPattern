using Core.Contracts;

namespace OrderWorker.Services;

public interface IOrderService : IJobService
{

}

public class OrderService : IOrderService
{
    public void Handle(Guid jobId)
    {
        Console.WriteLine("OrderService: came to handle method !!!");
    }
}

