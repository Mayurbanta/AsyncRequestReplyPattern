using Core.Contracts;

namespace TaxWorker.Services;

public interface ITaxService: IJobService
{

}

public class TaxService : ITaxService
{
    public void Handle(Guid jobId)
    {
        Console.WriteLine("TaxService: came to handle method !!!");
    }
}
